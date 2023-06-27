using AutoIt;
using Microsoft.Build.Framework;
using ParserRobot.DAL.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParserRobot.BLL.Workers
{
    public class Worker
    {
        public async Task StartWorkAsync()
        {
            AutoItX.Run("explorer.exe", "");
            //AutoItX.WinWaitActive("Проводник");
            Thread.Sleep(4000);


            AutoItX.Send("^f");
            Thread.Sleep(2000);
            AutoItX.Send($"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}");
            Thread.Sleep(2000);
            AutoItX.Send("{DOWN}");
            Thread.Sleep(2000);
            AutoItX.Send("{DOWN}");
            Thread.Sleep(2000);
            AutoItX.Send("{UP}");
            Thread.Sleep(2000);
            AutoItX.Send("{ENTER}");
            Thread.Sleep(2000);

            List<string> fileNames = Directory.GetFiles($"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}")
                                              .Select(x => Path.GetFileNameWithoutExtension(x))
                                              .ToList();
           
            
            List<string> IAfiles = new List<string>();
            List<string> MAfiles = new List<string>();

            foreach (string file in fileNames)
            {
                if(file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ИЭ"))
                    IAfiles.Add(file);

                if (file.StartsWith("РЕГИСТРАЦИЯ") && file.EndsWith("ТЭ"))
                    MAfiles.Add(file);
            }


            foreach (string file in IAfiles)
            {
            
                AutoItX.Send("^f");
                Thread.Sleep(2000);
                AutoItX.Send(file);
                AutoItX.Send("{ENTER}");
                Thread.Sleep(2000);
                AutoItX.Send("{DOWN}");
                Thread.Sleep(2000);
                AutoItX.Send("{DOWN}");
                Thread.Sleep(2000);
                AutoItX.Send("{ENTER}");
                Thread.Sleep(2000);
               
                AutoItX.Send("^a");
                Thread.Sleep(2000);
                AutoItX.Send("^c");
                Thread.Sleep(2000);
                string clipboardText = AutoItX.ClipGet();
                Thread.Sleep(2000);
                Debug.WriteLine(clipboardText);
                IAReader iAReader = new IAReader();
                iAReader.Read(clipboardText);
                if (!iAReader.IsCorrectData)
                {
                    string sourcePath = $"C:/Users/User/Desktop/{DateTime.Now.ToShortDateString()}/{file}.txt";
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
