using ParserRobot.DAL.Helpers;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using System;
using System.Text.RegularExpressions;

namespace ParserRobot.DAL.Readers
{
    public class MAReader : IReader<MerchantAcquiring>
    {
        public bool IsCorrectData { get; set; }

        public MerchantAcquiring Read(string text)
        {
            string idPattern = @"ID:\s+([a-f0-9-]+)\r?\n";
            string dailyTransactionLimitPattern = @"Лимит транзакций в сутки:\s+(\d+)\r?\n";
            string dailyTransactionAmountPattern = @"Кол-во сумма в сутки:\s+(\d+)\r?\n";
            string monthlyTransactionLimitPattern = @"Лимит транзакций в месяц:\s+(\d+)\r?\n";
            string monthlyTransactionAmountPattern = @"Кол-во сумма в месяц:\s+(\d+)\r?\n";
            string addressPattern = @"Адрес:\s+(.*?)\r?\n";
            string installationDatePattern = @"Дата установки:\s+(.*?)\r?\n";
            string licensePattern = @"Лицензия:\s+(.*?)\r?\n";
            string expirationDatePattern = @"Действут до:\s+(.*?)\r?\n";

            string datePattern = @"(\d{2}) (\w+) (\d{4})";

            MatchCollection matches = Regex.Matches(text, idPattern + "|" +
                                                          dailyTransactionLimitPattern + "|" +
                                                          dailyTransactionAmountPattern + "|" +
                                                          monthlyTransactionLimitPattern + "|" +
                                                          monthlyTransactionAmountPattern + "|" +
                                                          addressPattern + "|" +
                                                          installationDatePattern + "|" +
                                                          licensePattern + "|" +
                                                          expirationDatePattern);

            MerchantAcquiring MA = new MerchantAcquiring();

            foreach (Match match in matches)
            {
                Match installationDateMatch = Regex.Match(match.Groups[7].Value, datePattern);
                Match expirationDateMatch = Regex.Match(match.Groups[9].Value, datePattern);

                if (match.Groups[1].Success) MA.Id = Guid.Parse(match.Groups[1].Value);
                if (match.Groups[2].Success) MA.DayLimit = int.Parse(match.Groups[2].Value);
                if (match.Groups[3].Success) MA.AmountPerDay = decimal.Parse(match.Groups[3].Value);
                if (match.Groups[4].Success) MA.MonthLimit = int.Parse(match.Groups[4].Value);
                if (match.Groups[5].Success) MA.AmountPerMonth = decimal.Parse(match.Groups[5].Value);
                if (match.Groups[6].Success) MA.Adress = match.Groups[6].Value;
                if (match.Groups[8].Success) MA.License = match.Groups[8].Value;

                if (installationDateMatch.Success)
                {
                    int day = int.Parse(installationDateMatch.Groups[1].Value);
                    string monthString = installationDateMatch.Groups[2].Value;
                    int year = int.Parse(installationDateMatch.Groups[3].Value);
                    DateTime installationDate = new DateTime(year, MonthNumberHelper.GetMonthNumber(monthString), day);
                    MA.InstallationDate = installationDate;
                }
                if (expirationDateMatch.Success)
                {
                    int day = int.Parse(expirationDateMatch.Groups[1].Value);
                    string monthString = expirationDateMatch.Groups[2].Value;
                    int year = int.Parse(expirationDateMatch.Groups[3].Value);
                    DateTime licenseExpirationDate = new DateTime(year, MonthNumberHelper.GetMonthNumber(monthString), day);
                    MA.LicenseExpirationDate = licenseExpirationDate;
                }
            }

            if (MA.InstallationDate != null)
            {
                IsCorrectData = true;
                return MA;
            }
            else IsCorrectData = false;

            return null;
        }
    }
}
