using ParserRobot.DAL.ModelsDAO;
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
            //TestingIA();
            TestingMA();
        }
        private static void TestingMA()
        {
            List<MerchantAcquiring> merchantAcquirings = new List<MerchantAcquiring>();
            MAReader reader = new MAReader();
            //string text = "Наименование: Терминал торгового эквайринга\r\nID: f2f3413b-21f8-412a-b601-912e58a173f7\r\nЛимит транзакций в сутки: 30\r\nКол-во сумма в сутки: 60000\r\nЛимит транзакций в месяц: 900\r\nКол-во сумма в месяц: 1800000\r\nОтветственный сотрудник: Мышковец Алина Петровна\r\nАдрес: Республика Беларусь, г. Минск, ул. Жыбренко 9д оф 35\r\nОрганизация: Индивидуальный предприниматель Петров Петр Петрович\r\nДата установки: 23 апреля 2023\r\nЛицензия: Есть\r\nДействут до: 23 апреля 2024";
            //string text = "Наименование: Терминал торгового эквайринга\r\nID: 5649651d-0383-4640-b76d-cef129bf502c\r\nЛимит транзакций в сутки: 15\r\nКол-во сумма в сутки: 721827\r\nЛимит транзакций в месяц: 213\r\nКол-во сумма в месяц: 412421\r\nАдрес: Республика Беларусь, г. Минск, ул.\r\nОрганизация: Закрытое акционерное\r\nобщесто \"Ювелирный салон именни Альберта\r\nИвановича\"\r\nДата установки: 23 апреля 2023\r\n";
            string text = "Наименование: Терминал торгового эквайринга\r\nОрганизация: ИП Жмых Петр Петрович\r\nID: 5649651d-0183-4640-b21d-c421412421dwa\r\nЛимит транзакций в сутки: 15\r\nКол-во сумма в сутки: 721827\r\nЛимит транзакций в месяц: 213\r\nКол-во сумма в месяц: 412421\r\nАдрес: Республика Беларусь, г. Минск, ул. Петруся 3\r\n\r\n";
            reader.Read(text);
            Console.WriteLine(reader.IsCorrectData);
            //if (reader.IsCorrectData)
            //{
            //    MAWriter writer = new MAWriter();
            //    merchantAcquirings.Add(reader.Read(text));
            //    writer.Write(merchantAcquirings);
            //}
            Console.ReadKey();
        }


        private static void TestingIA()
        {
            List<InternetAcquiring> internetAcquirings = new List<InternetAcquiring>();
            IAReader reader = new IAReader();
            //string text = "Наименование: https:\\\\github.com\\\r\nТерминалID: 134221\r\nУНП: 01092031\r\nБаза: C22300C\r\nMERCHANTID: 63bec124-4af6-46da-a582-376ad554bde9\r\nСумма в день: 1200\r\nКол-во в день: 300\r\nСумма в месяц: 45000\r\nКол-во в месяц: 5000\r\nПолное наименование: GitHub — крупнейший веб-сервис для хостинга IT-проектов и их совместной разработки.\r\nДата создания: 01 мая 2023 года 19:01:001\r\nСрок действия: 5 лет";
            string text = "Наименование: https:\\\\onliner.by\\\r\nДата создания: 23 апреля 2023 года\r\nMERCHANTID: 4008650c-5bd6-4934-9107-e49edb6872ba\r\nТерминалID: 134221\r\nБаза: 0101010\r\nСумма в день: 550\r\nКол-во в день: 30\r\nСумма в месяц: 950\r\nПолное наименование: Общество с ограниченной ответственностью \"Онлайнер\"\r\nУНП: 21841242\r\nСрок действия: 2 года";
            //string text = "Наименование: https:\\\\тестой.бай\\\r\nДата создания: 4 января 2023 года\r\nMERCHANTID: 421412aw-4dwa2-4934-9107-e49edb6872ba\r\nТерминалID: 134221\r\nБаза: 0101010\r\nСумма в день: 550\r\nКол-во в день: 30\r\nСумма в месяц: 950\r\nПолное наименование: Частное торговое унитарное предприятие \"Тесто\"\r\nСрок действия: 1 год";
            reader.Read(text);
            Console.WriteLine(reader.IsCorrectData);
            if (reader.IsCorrectData)
            {
                IAWriter writer = new IAWriter();
                internetAcquirings.Add(reader.Read(text));
                writer.Write(internetAcquirings);
            }
            //if (reader.IsCorrectData)
            //{
            //    IAWriter writer = new IAWriter();
            //    writer.Write(reader.Read(text));
            //}

            Console.ReadKey();
        }
    }
}
