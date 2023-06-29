using System;

namespace ParserRobot.DAL.Helpers
{
    public static class MonthNumberHelper
    {
        public static int GetMonthNumber(string monthString)
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