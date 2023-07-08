using AutoIt;
using Microsoft.Extensions.Logging;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using ParserRobot.DAL.Writers.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParserRobot.BLL.Workers
{
    public class Worker
    {
        private readonly IReader<InternetAcquiring> _iaReader;
        private readonly IReader<MerchantAcquiring> _maReader;
        private readonly IWriter<InternetAcquiring> _iaWriter;
        private readonly IWriter<MerchantAcquiring> _maWriter;
        private readonly ILogger<Worker> _logger;

        private int _timeToOtherActions = 2000;
        //private int _timeToWaitNewFiles = 1800000;
        private int _timeToWaitNewFiles = 30000;

        private string _clipboardText;
        private string _pathToDekstopDateDirectory;
        private string _dateNowString = DateTime.Now.ToShortDateString();
        private string _pathWithSaveReadFileNames;

        private List<string> _processedFiles = new List<string>();
        private List<string> _fileNames = new List<string>();

        private List<InternetAcquiring> _internetAcquirings = new List<InternetAcquiring>();
        private List<MerchantAcquiring> _merchantAcquiring = new List<MerchantAcquiring>();

        public Worker(ILogger<Worker> logger,
                      IReader<InternetAcquiring> iaReader,
                      IReader<MerchantAcquiring> maReader,
                      IWriter<InternetAcquiring> iaWriter,
                      IWriter<MerchantAcquiring> maWriter)
        {
            _logger = logger;
            _iaReader = iaReader;
            _maReader = maReader;
            _iaWriter = iaWriter;
            _maWriter = maWriter;
            _pathToDekstopDateDirectory = ConfigurationManager.AppSettings["dekstopPath"].ToString() + _dateNowString;
            _pathWithSaveReadFileNames = ConfigurationManager.AppSettings["historyPath"].ToString() + $"ReadFileNames {_dateNowString}.txt";
        }

        public async Task StartWorkAsync()
        {
            //AutoItX.AutoItSetOption("SendKeyDelay", 1000);

            AutoItX.Run("explorer.exe", "");
            _logger.LogInformation($"Открытие проводника");
            Thread.Sleep(_timeToOtherActions);

            while (true)
            {
                _logger.LogInformation("Ищу файлы для обработки");
                await SearchFilesToProcess();

                _logger.LogInformation("Файлов для обработки нет. Засыпаю на 30 мин");
                await Task.Delay(_timeToWaitNewFiles);
            }
        }

        private async Task SearchFilesToProcess()
        {
            _fileNames = Directory.GetFiles(_pathToDekstopDateDirectory)
                              .Select(x => Path.GetFileNameWithoutExtension(x))
                              .Where(file => file.StartsWith("РЕГИСТРАЦИЯ") && (file.EndsWith("ИЭ") || file.EndsWith("ТЭ")))
                              .ToList();

            if (!IsFileExists(_pathWithSaveReadFileNames)) File.Create(_pathWithSaveReadFileNames).Close();

            _processedFiles = File.ReadAllLines(_pathWithSaveReadFileNames).ToList();

            foreach (string file in _fileNames)
            {
                if (!_processedFiles.Contains(file))
                {
                    if (AutoItX.WinExists("Главная") == 0)
                    {
                        AutoItX.Run("explorer.exe", "");
                        _logger.LogInformation($"Открытие проводника");
                        Thread.Sleep(_timeToOtherActions);
                    }
                    _logger.LogInformation($"Начинаю обработку файла {file}");
                    await WriteAtFile(_pathWithSaveReadFileNames, file);
                    await ProcessFile(file);
                }
            }
        }

        private async Task ProcessFile(string file)
        {
            AutoItX.Send("^l");
            _logger.LogInformation("Активация строки поиска");
            Thread.Sleep(_timeToOtherActions);

            AutoItX.Send($"{_pathToDekstopDateDirectory}/{file}.txt");
            _logger.LogInformation("Ввод в строку поиска названия нужного файла");
            AutoItX.Send("{ENTER}");
            _logger.LogInformation("Нажимаем клавишу ENTER");
            Thread.Sleep(_timeToOtherActions);

            AutoItX.Send("^a");
            _logger.LogInformation("Нажимаем сочетание клавиш Сrtl+A");
            Thread.Sleep(_timeToOtherActions);

            AutoItX.Send("^c");
            _logger.LogInformation("Нажимаем сочетание клавиш Сrtl+C");
            Thread.Sleep(_timeToOtherActions);

            _clipboardText = AutoItX.ClipGet();
            _logger.LogInformation("Получаем данные из буфера обмена");
            _logger.LogInformation(_clipboardText);
            Thread.Sleep(_timeToOtherActions);

            _logger.LogInformation("Проверка по какому шаблону нужно обработать данный файл (ИЭ или ТЭ)");
            if (file.EndsWith("ИЭ"))
            {
                _internetAcquirings.Add(_iaReader.Read(_clipboardText));

                if (_iaReader.IsCorrectData) _iaWriter.Write(_internetAcquirings);
                else await GetErrorData(file);

                _logger.LogInformation($"Коректность данных в файле {file}: {_iaReader.IsCorrectData}");
            }
            else
            {
                _merchantAcquiring.Add(_maReader.Read(_clipboardText));

                if (_maReader.IsCorrectData) _maWriter.Write(_merchantAcquiring);
                else await GetErrorData(file);

                _logger.LogInformation($"Коректность данных в файле {file}: {_maReader.IsCorrectData}");
            }

            AutoItX.WinKill($"{file}");
            _logger.LogInformation($"Закрываем окно файла {file}");
        }

        private async Task GetErrorData(string fileName)
        {
            string sourcePath = $"{_pathToDekstopDateDirectory}/{fileName}.txt";
            string destinationPath = ConfigurationManager.AppSettings["errorsPath"].ToString() + $"Errors {_dateNowString}/{fileName}Error.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            await Task.Run(() => File.Copy(sourcePath, destinationPath, overwrite: true));
        }

        private bool IsFileExists(string file) => File.Exists(file);

        private async Task WriteAtFile(string filePath, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    await writer.WriteLineAsync(fileName);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при записи в файл: {ex.Message}");
                throw new Exception($"Ошибка при записи в файл: {ex.Message}");
            }
        }
    }
}