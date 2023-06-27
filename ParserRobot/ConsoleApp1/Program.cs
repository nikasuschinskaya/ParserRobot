using ParserRobot.DAL.Readers;
using ParserRobot.DAL.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {

        static void Main(string[] args)
        {
            IAReader reader = new IAReader();
            string text = "Наименование: https:\\\\github.com\\\r\nТерминалID: 134221\r\nУНП: 01092031\r\nБаза: C22300C\r\nMERCHANTID: 63bec124-4af6-46da-a582-376ad554bde9\r\nСумма в день: 1200\r\nКол-во в день: 300\r\nСумма в месяц: 45000\r\nКол-во в месяц: 5000\r\nПолное наименование: GitHub — крупнейший веб-сервис для хостинга IT-проектов и их совместной разработки.\r\nДата создания: 01 мая 2023 года 19:01:001\r\nСрок действия: 5 лет";
            //string text = "Наименование: https:\\\\onliner.by\\\r\nДата создания: 23 апреля 2023 года\r\nMERCHANTID: 4008650c-5bd6-4934-9107-e49edb6872ba\r\nТерминалID: 134221\r\nБаза: 0101010\r\nСумма в день: 550\r\nКол-во в день: 30\r\nСумма в месяц: 950\r\nПолное наименование: Общество с ограниченной ответственностью \"Онлайнер\"\r\nУНП: 21841242\r\nСрок действия: 2 года";
            //string text = "Наименование: https:\\\\тестой.бай\\\r\nДата создания: 4 января 2023 года\r\nMERCHANTID: 421412aw-4dwa2-4934-9107-e49edb6872ba\r\nТерминалID: 134221\r\nБаза: 0101010\r\nСумма в день: 550\r\nКол-во в день: 30\r\nСумма в месяц: 950\r\nПолное наименование: Частное торговое унитарное предприятие \"Тесто\"\r\nСрок действия: 1 год";
            reader.Read(text);
            //if (reader.IsCorrectData)
            //{
            //    IAWriter writer = new IAWriter();
            //    writer.Write(reader.Read(text));
            //}
            Console.WriteLine(reader.IsCorrectData);
            Console.ReadKey();
        }
    }
}
