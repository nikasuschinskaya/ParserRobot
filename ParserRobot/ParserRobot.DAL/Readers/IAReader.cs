using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParserRobot.DAL.Readers
{
    public class IAReader : IReader<InternetAcquiring>
    {
        public bool IsCorrectData { get; set; }
        public InternetAcquiring Read(string text)
        {
            string pattern = @"Наименование:\s+(.*?)\r?\n" +
                                @"ТерминалID:\s+(\d+)\r?\n" +
                                @"УНП:\s+(\d+)\r?\n" +
                                @"База:\s+(\w+)\r?\n" +
                                @"MERCHANTID:\s+([a-f0-9-]+)\r?\n" +
                                @"Сумма в день:\s+(\d+)\r?\n" +
                                @"Кол-во в день:\s+(\d+)\r?\n" +
                                @"Сумма в месяц:\s+(\d+)\r?\n" +
                                @"Кол-во в месяц:\s+(\d+)\r?\n" +
                                @"Полное наименование:\s+(.*?)\r?\n" +
                                @"Дата создания:\s+(.*?)\r?\n" +
                                @"Срок действия:\s+(\d+)\s+(лет|год|года)";

            string datePattern = @"(\d{2}) (\w+) (\d{4}) года (\d{2}):(\d{2}):(\d{2})";

            //List<InternetAcquiring> result = new List<InternetAcquiring>();

            Match match = Regex.Match(text, pattern);

            //foreach (var IA in result)
            //{

            InternetAcquiring IA = new InternetAcquiring();

            Match dateMatch = Regex.Match(match.Groups[11].Value, datePattern);

            if (dateMatch.Success)
            {
                int day = int.Parse(dateMatch.Groups[1].Value);
                string monthString = dateMatch.Groups[2].Value;
                int year = int.Parse(dateMatch.Groups[3].Value);
                int hour = int.Parse(dateMatch.Groups[4].Value);
                int minute = int.Parse(dateMatch.Groups[5].Value);
                int second = int.Parse(dateMatch.Groups[6].Value);

                DateTime creationDate = new DateTime(year, GetMonthNumber(monthString), day, hour, minute, second);
                IA.CreationDate = creationDate;
            }

            if (match.Success)
            {
                IsCorrectData = true;
               
                IA.FullName = match.Groups[10].Value;
                IA.PayerAccountNumber = match.Groups[3].Value;
                //IA.CreationDate = DateTime.Parse(match.Groups[11].Value);
                IA.AmountPerDay = decimal.Parse(match.Groups[6].Value);
                IA.CountPerDay = int.Parse(match.Groups[7].Value);
                IA.AmountPerMonth = decimal.Parse(match.Groups[8].Value);
                IA.CountPerMonth = int.Parse(match.Groups[9].Value);
                //result.Add(IA);
                    
            }
            else IsCorrectData = false;
            
            return null;
            //return result;
        }

        private int GetMonthNumber(string monthString)
        {
            switch (monthString.ToLower())
            {
                case "января":
                    return 1;
                case "февраля":
                    return 2;
                case "марта":
                    return 3;
                case "апреля":
                    return 4;
                case "мая":
                    return 5;
                case "июня":
                    return 6;
                case "июля":
                    return 7;
                case "августа":
                    return 8;
                case "сентября":
                    return 9;
                case "октября":
                    return 10;
                case "ноября":
                    return 11;
                case "декабря":
                    return 12;
                default:
                    throw new ArgumentException("Недопустимое название месяца: " + monthString);
            }
        }
    }
}
