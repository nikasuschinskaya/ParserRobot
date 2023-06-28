using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ParserRobot.DAL.Readers
{
    public class IAReader : IReader<InternetAcquiring>
    {
        public bool IsCorrectData { get; set; }
        public List<InternetAcquiring> Read(string text)
        {
            string unpPattern = @"УНП:\s+(\d+)\r?\n";
            string amountPerDayPattern = @"Сумма в день:\s+(\d+)\r?\n";
            string countPerDayPattern = @"Кол-во в день:\s+(\d+)\r?\n";
            string amountPerMonthPattern = @"Сумма в месяц:\s+(\d+)\r?\n";
            string countPerMonthPattern = @"Кол-во в месяц:\s+(\d+)\r?\n";
            string fullNamePattern = @"Полное наименование:\s+(.*?)\r?\n";
            string creationDatePattern = @"Дата создания:\s+(.*?)\r?\n";

            string datePattern = @"(\d{2}) (\w+) (\d{4}) года";

            List<InternetAcquiring> result = new List<InternetAcquiring>();

            MatchCollection matches = Regex.Matches(text, unpPattern + "|" + amountPerDayPattern + "|" + countPerDayPattern + "|" + amountPerMonthPattern + "|" + countPerMonthPattern + "|" + fullNamePattern + "|" + creationDatePattern);

            InternetAcquiring IA = new InternetAcquiring();

            foreach (Match match in matches)
            {
                Match dateMatch = Regex.Match(match.Groups[7].Value, datePattern);
                if (match.Groups[1].Success)
                {
                    IA.PayerAccountNumber = match.Groups[1].Value;
                }
                else if (match.Groups[2].Success)
                {
                    IA.AmountPerDay = decimal.Parse(match.Groups[2].Value);
                }
                else if (match.Groups[3].Success)
                {
                    IA.CountPerDay = int.Parse(match.Groups[3].Value);
                }
                else if (match.Groups[4].Success)
                {
                    IA.AmountPerMonth = decimal.Parse(match.Groups[4].Value);
                }
                else if (match.Groups[5].Success)
                {
                    IA.CountPerMonth = int.Parse(match.Groups[5].Value);
                }
                else if (match.Groups[6].Success)
                {
                    IA.FullName = match.Groups[6].Value;
                }
                else if(dateMatch.Success)
                {
                    int day = int.Parse(dateMatch.Groups[1].Value);
                    string monthString = dateMatch.Groups[2].Value;
                    int year = int.Parse(dateMatch.Groups[3].Value);
                    DateTime creationDate = new DateTime(year, GetMonthNumber(monthString), day);
                    IA.CreationDate = creationDate;
                }
            }

            if (IA.PayerAccountNumber != null /*&& IA.AmountPerDay != 0 && IA.CountPerDay != 0 && IA.AmountPerMonth != 0 && IA.CountPerMonth != 0 && IA.FullName != null*/)
            {
                result.Add(IA);
            }

            if (result.Count == 0) IsCorrectData = false;
            else IsCorrectData = true;

            return result;
        }

        private int GetMonthNumber(string monthString)
        {
            switch (monthString.ToLower())
            {
                case "января": return 1;
                case "февраля": return 2;
                case "марта": return 3;
                case "апреля": return 4;
                case "мая": return 5;
                case "июня": return 6;
                case "июля": return 7;
                case "августа": return 8;
                case "сентября": return 9;
                case "октября": return 10;
                case "ноября": return 11;
                case "декабря": return 12;
                default: throw new ArgumentException("Недопустимое название месяца: " + monthString);
            }
        }
    }
}
