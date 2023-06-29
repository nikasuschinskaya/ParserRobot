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
        private int _timeForOpenExplorer = 4000;
        private int _timeToOtherActions = 2000;
        private string _clipboardText;
        //private string _line;
        private string _pathToDekstopDateDirectory = $"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}";
        private string _dateNowString = DateTime.Now.ToShortDateString();
        private string _pathWithSaveReadFileNames = "FileNames/ReadFileNames.txt";
        //private List<string> _saveReadFileLines = new List<string>();
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

            List<string> fileNames = Directory.GetFiles(_pathToDekstopDateDirectory)
                                              .Select(x => Path.GetFileNameWithoutExtension(x))
                                              .Where(file => file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ИЭ") ||
                                                     file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ТЭ"))
                                              .ToList();

            foreach (string file in fileNames)
            {
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
        }

        private async Task GetErrorData(string fileName)
        {
            string sourcePath = $"{_pathToDekstopDateDirectory}/{fileName}.txt";
            string destinationPath = $"C:/Users/User/source/repos/ParserRobot/ParserRobot/ParserRobot.UI/Errors/Errors {_dateNowString}/{fileName}Error.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            await Task.Run(() => File.Copy(sourcePath, destinationPath, overwrite: true));
        }
    }
}
