﻿using IronXL;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Writers.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ParserRobot.DAL.Writers
{
    public class MAWriter : IWriter<MerchantAcquiring>
    {
        private int _widthForColumns = 3800;
        private int _widthForMoreBigColumns = 4500;
        private int _widthForAColumn = 10000;
        private static string _dateString = DateTime.Now.ToShortDateString();
        private string _pathForSaveExelFile = ConfigurationManager.AppSettings["reportsMAPath"].ToString() + $"Report MerchantAcquiring {_dateString}.xls";

        public void Write(List<MerchantAcquiring> models)
        {
            WorkBook xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLS);
            WorkSheet xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");

            xlsSheet["A1"].Value = "ID";
            xlsSheet["B1"].Value = "Лимит сутки";
            xlsSheet["C1"].Value = "Кол-во сумма сутки";
            xlsSheet["D1"].Value = "Лимит месяц";
            xlsSheet["E1"].Value = "Кол-во сумма месяц";
            xlsSheet["F1"].Value = "Точка установки";
            xlsSheet["G1"].Value = "Дата установки";
            xlsSheet["H1"].Value = "Лицензия";
            xlsSheet["I1"].Value = "Дата окончания лицензии";

            xlsSheet.Columns[0].Width = _widthForAColumn;
            xlsSheet.Columns[1].Width = _widthForColumns;
            xlsSheet.Columns[2].Width = _widthForMoreBigColumns;
            xlsSheet.Columns[3].Width = _widthForColumns;
            xlsSheet.Columns[4].Width = _widthForMoreBigColumns;
            xlsSheet.Columns[5].Width = _widthForAColumn + _widthForColumns;
            xlsSheet.Columns[6].Width = _widthForColumns;
            xlsSheet.Columns[7].Width = _widthForColumns;
            xlsSheet.Columns[8].Width = _widthForAColumn - _widthForColumns;

            for (int i = 0; i < models.Count(); i++)
            {
                xlsSheet[$"A{i + 2}"].Value = models[i].Id.ToString();
                xlsSheet[$"B{i + 2}"].Value = models[i].DayLimit.ToString();
                xlsSheet[$"C{i + 2}"].Value = models[i].AmountPerDay.ToString();
                xlsSheet[$"D{i + 2}"].Value = models[i].MonthLimit.ToString();
                xlsSheet[$"E{i + 2}"].Value = models[i].AmountPerMonth.ToString();
                xlsSheet[$"F{i + 2}"].Value = models[i].Adress.ToString();
                xlsSheet[$"G{i + 2}"].Value = models[i].InstallationDate.ToString();
                if (models[i].License != null)
                {
                    xlsSheet[$"H{i + 2}"].Value = "да";
                    xlsSheet[$"I{i + 2}"].Value = models[i].LicenseExpirationDate.ToString();
                }
                else
                {
                    xlsSheet[$"H{i + 2}"].Value = "нет";
                    xlsSheet[$"I{i + 2}"].Value = string.Empty;
                }
            }

            xlsWorkbook.SaveAs(_pathForSaveExelFile);
        }
    }
}
