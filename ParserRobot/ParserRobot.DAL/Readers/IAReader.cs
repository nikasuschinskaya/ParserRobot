using ParserRobot.DAL.Helpers;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using System;
using System.Text.RegularExpressions;

namespace ParserRobot.DAL.Readers
{
    public class IAReader : IReader<InternetAcquiring>
    {
        public bool IsCorrectData { get; set; }

        public InternetAcquiring Read(string text)
        {
            string unpPattern = @"УНП:\s+(\d+)\r?\n";
            string amountPerDayPattern = @"Сумма в день:\s+(\d+)\r?\n";
            string countPerDayPattern = @"Кол-во в день:\s+(\d+)\r?\n";
            string amountPerMonthPattern = @"Сумма в месяц:\s+(\d+)\r?\n";
            string countPerMonthPattern = @"Кол-во в месяц:\s+(\d+)\r?\n";
            string fullNamePattern = @"Полное наименование:\s+(.*?)\r?\n";
            string creationDatePattern = @"Дата создания:\s+(.*?)\r?\n";

            string datePattern = @"(\d{2}) (\w+) (\d{4}) года";

            MatchCollection matches = Regex.Matches(text, unpPattern + "|" + 
                                                          amountPerDayPattern + "|" + 
                                                          countPerDayPattern + "|" + 
                                                          amountPerMonthPattern + "|" + 
                                                          countPerMonthPattern + "|" + 
                                                          fullNamePattern + "|" + 
                                                          creationDatePattern);

            InternetAcquiring IA = new InternetAcquiring();

            foreach (Match match in matches)
            {
                Match dateMatch = Regex.Match(match.Groups[7].Value, datePattern);

                if (match.Groups[1].Success) IA.PayerAccountNumber = match.Groups[1].Value;
                else if (match.Groups[2].Success) IA.AmountPerDay = decimal.Parse(match.Groups[2].Value);
                else if (match.Groups[3].Success) IA.CountPerDay = int.Parse(match.Groups[3].Value);
                else if (match.Groups[4].Success) IA.AmountPerMonth = decimal.Parse(match.Groups[4].Value);
                else if (match.Groups[5].Success) IA.CountPerMonth = int.Parse(match.Groups[5].Value);
                else if (match.Groups[6].Success) IA.FullName = match.Groups[6].Value;

                else if (dateMatch.Success)
                {
                    int day = int.Parse(dateMatch.Groups[1].Value);
                    string monthString = dateMatch.Groups[2].Value;
                    int year = int.Parse(dateMatch.Groups[3].Value);
                    DateTime creationDate = new DateTime(year, MonthNumberHelper.GetMonthNumber(monthString), day);
                    IA.CreationDate = creationDate;
                }
            }

            if (IA.PayerAccountNumber != null)
            {
                IsCorrectData = true;
                return IA;
            }
            else IsCorrectData = false;

            return null;
        }
    }
}