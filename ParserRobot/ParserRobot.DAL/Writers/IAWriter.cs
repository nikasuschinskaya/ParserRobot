using IronXL;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Registrators;
using ParserRobot.DAL.Writers.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ParserRobot.DAL.Writers
{
    public class IAWriter : IWriter<InternetAcquiring>
    {
        private int _widthForColumns = 3800;
        private int _widthForAColumn = 20000;
        private static string _dateString = DateTime.Now.ToShortDateString();
        private string _pathForSaveExelFile = ConfigurationManager.AppSettings["reportsIAPath"].ToString() + $"Report InternetAcquiring {_dateString}.xls";

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

            xlsSheet.Columns[0].Width = _widthForAColumn;
            xlsSheet.Columns[2].Width = _widthForColumns;
            xlsSheet.Columns[3].Width = _widthForColumns;
            xlsSheet.Columns[4].Width = _widthForColumns;
            xlsSheet.Columns[5].Width = _widthForColumns;
            xlsSheet.Columns[6].Width = _widthForColumns;
            xlsSheet.Columns[7].Width = _widthForColumns;

            for (int i = 0; i < models.Count(); i++)
            {
                xlsSheet[$"A{i + 2}"].Value = models[i].FullName.ToString();
                xlsSheet[$"B{i + 2}"].Value = models[i].PayerAccountNumber.ToString();
                xlsSheet[$"C{i + 2}"].Value = models[i].CreationDate.ToShortDateString();
                xlsSheet[$"D{i + 2}"].Value = models[i].AmountPerDay.ToString();
                xlsSheet[$"E{i + 2}"].Value = models[i].CountPerDay.ToString();
                xlsSheet[$"F{i + 2}"].Value = models[i].AmountPerMonth.ToString();
                xlsSheet[$"G{i + 2}"].Value = models[i].CountPerMonth.ToString();
                xlsSheet[$"I{i + 2}"].Value = "работает";
            }

            xlsWorkbook.SaveAs(_pathForSaveExelFile);
        }
    }
}