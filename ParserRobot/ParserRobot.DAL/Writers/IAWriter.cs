using IronXL;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Writers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserRobot.DAL.Writers
{
    public class IAWriter : IWriter<InternetAcquiring>
    {
        public void Write(List<InternetAcquiring> models)
        {
            WorkBook xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLS);
            WorkSheet xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");
            xlsSheet["A1"].Value = "Наименование";
            xlsSheet["B1"].Value = "УНП";
            xlsSheet["C1"].Value = "Дата создания";
            xlsSheet["D1"].Value = "Сумма в день";
            xlsSheet["E1"].Value = "Кол-во в день";
            xlsSheet["F1"].Value = "Сумма в месяц";
            xlsSheet["G1"].Value = "Кол-во в месяц";
            xlsSheet["H1"].Value = "Ответственный";
            xlsSheet["I1"].Value = "Статус";

            for(int i = 0; i < models.Count(); i++)
            {
                xlsSheet[$"A{i + 2}"].Value = models[i].FullName.ToString();
                xlsSheet[$"B{i + 2}"].Value = models[i].PayerAccountNumber.ToString();
                xlsSheet[$"C{i + 2}"].Value = models[i].CreationDate.ToShortDateString();
                xlsSheet[$"D{i + 2}"].Value = models[i].AmountPerDay.ToString();
                xlsSheet[$"E{i + 2}"].Value = models[i].CountPerDay.ToString();
                xlsSheet[$"F{i + 2}"].Value = models[i].AmountPerMonth.ToString();
                xlsSheet[$"G{i + 2}"].Value = models[i].CountPerMonth.ToString();
                xlsSheet[$"I{i + 2}"].Value = "работает";
                //xlsSheet[$"H1"].Value = "Ответственный";
            }

            xlsWorkbook.SaveAs($"C:/Users/User/source/repos/ParserRobot/ParserRobot/ParserRobot.UI/Reports/Report InternetAcquiring {DateTime.Now.ToShortDateString()}.xls");
           
        }
    }
}
