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
        private string _pathToDekstopDateDirectory = $"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}";
        private List<InternetAcquiring> _internetAcquirings = new List<InternetAcquiring>();

        public async Task StartWorkAsync()
        {

            AutoItX.Run("explorer.exe", "");
            Thread.Sleep(_timeForOpenExplorer);
            AutoItX.Send("^f");
            Thread.Sleep(_timeToOtherActions);
            AutoItX.Send(_pathToDekstopDateDirectory);
            Thread.Sleep(_timeToOtherActions);
            AutoItX.Send("{DOWN}");
            Thread.Sleep(_timeToOtherActions);
            AutoItX.Send("{DOWN}");
            Thread.Sleep(_timeToOtherActions);
            AutoItX.Send("{UP}");
            Thread.Sleep(_timeToOtherActions);
            AutoItX.Send("{ENTER}");
            Thread.Sleep(_timeToOtherActions);

            List<string> fileNames = Directory.GetFiles(_pathToDekstopDateDirectory)
                                              .Select(x => Path.GetFileNameWithoutExtension(x))
                                              .ToList();

            List<string> IAfiles = fileNames.Where(file => file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ИЭ")).ToList();
            List<string> MAfiles = fileNames.Where(file => file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ТЭ")).ToList();

            foreach (string file in IAfiles)
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
                IAReader iAReader = new IAReader();

                _internetAcquirings.Add(iAReader.Read(_clipboardText));

                if (iAReader.IsCorrectData)
                {
                    IAWriter iAWriter = new IAWriter();
                    iAWriter.Write(_internetAcquirings);
                }
                else
                {
                    string sourcePath = $"{_pathToDekstopDateDirectory}/{file}.txt";
                    string destinationPath = $"C:/Users/User/source/repos/ParserRobot/ParserRobot/ParserRobot.UI/Errors/Errors {DateTime.Now.ToShortDateString()}/{file}Error.txt";

                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    File.Copy(sourcePath, destinationPath, overwrite: true);
                }

                Debug.WriteLine(iAReader.IsCorrectData);
                AutoItX.WinKill($"{file}");

            }
        }
    }
}
