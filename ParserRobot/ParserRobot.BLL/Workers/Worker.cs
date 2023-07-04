using AutoIt;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers;
using ParserRobot.DAL.Writers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParserRobot.BLL.Workers
{
    public class Worker
    {
        private int _timeToOtherActions = 2000;
        private int _timeToWaitNewFiles = 1800000;

        private string _clipboardText;
        private string _pathToDekstopDateDirectory = $"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}";
        private string _dateNowString = DateTime.Now.ToShortDateString();
        private string _pathWithSaveReadFileNames = $"C:/Users/User/source/repos/ParserRobot/ParserRobot/ParserRobot.UI/HistoryReadFiles/ReadFileNames {DateTime.Now.ToShortDateString()}.txt";

        private List<string> _processedFiles = new List<string>();
        private List<string> _fileNames = new List<string>();

        private List<InternetAcquiring> _internetAcquirings = new List<InternetAcquiring>();
        private List<MerchantAcquiring> _merchantAcquiring = new List<MerchantAcquiring>();

        public async Task StartWorkAsync()
        {
            AutoItX.Send("^{ESC}");
            Thread.Sleep(_timeToOtherActions);

            AutoItX.Send(_dateNowString);
            Thread.Sleep(_timeToOtherActions);

            AutoItX.Send("{ENTER}");
            Thread.Sleep(_timeToOtherActions);

            _fileNames = Directory.GetFiles(_pathToDekstopDateDirectory)
                                  .Select(x => Path.GetFileNameWithoutExtension(x))
                                  .Where(file => file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ИЭ") ||
                                         file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ТЭ"))
                                  .ToList();

            if(!IsFileExists(_pathWithSaveReadFileNames)) File.Create(_pathWithSaveReadFileNames);

            _processedFiles = File.ReadAllLines(_pathWithSaveReadFileNames).ToList();

            foreach (string file in _fileNames)
            {
                if (!_processedFiles.Contains(file))
                {
                    await WriteAtFile(_pathWithSaveReadFileNames, file);

                    AutoItX.Send("^f");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send(file);
                    AutoItX.Send("{ENTER}");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send("{DOWN}");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send("{DOWN}");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send("{ENTER}");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send("^a");
                    Thread.Sleep(_timeToOtherActions);

                    AutoItX.Send("^c");
                    Thread.Sleep(_timeToOtherActions);

                    _clipboardText = AutoItX.ClipGet();
                    Thread.Sleep(_timeToOtherActions);

                    Debug.WriteLine(_clipboardText);

                    if (file.EndsWith("ИЭ"))
                    {
                        IAReader iAReader = new IAReader();
                        _internetAcquirings.Add(iAReader.Read(_clipboardText));
                        if (iAReader.IsCorrectData)
                        {
                            IAWriter iAWriter = new IAWriter();
                            iAWriter.Write(_internetAcquirings);
                        }
                        else await GetErrorData(file);
                        Debug.WriteLine(iAReader.IsCorrectData);
                    }
                    else
                    {
                        MAReader maReader = new MAReader();
                        _merchantAcquiring.Add(maReader.Read(_clipboardText));
                        if (maReader.IsCorrectData)
                        {
                            MAWriter maWriter = new MAWriter();
                            maWriter.Write(_merchantAcquiring);
                        }
                        else await GetErrorData(file);
                        Debug.WriteLine(maReader.IsCorrectData);
                    }
                    AutoItX.WinKill($"{file}");
                }
                else Thread.Sleep(_timeToWaitNewFiles);
            }
        }

        private async Task GetErrorData(string fileName)
        {
            string sourcePath = $"{_pathToDekstopDateDirectory}/{fileName}.txt";
            string destinationPath = $"C:/Users/User/source/repos/ParserRobot/ParserRobot/ParserRobot.UI/Errors/Errors {_dateNowString}/{fileName}Error.txt";

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
                throw new Exception($"Ошибка при записи в файл: {ex.Message}");
            }
        }
    }
}