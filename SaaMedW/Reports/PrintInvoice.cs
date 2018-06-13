using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Packaging;

namespace SaaMedW
{
    public class Report
    {
        public Band Header { get; set; }
        public Band Detail { get; set; }
        public Band Footer { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
    public class Group
    {
        public string expr { get; set; }
        public Band Header { get; set; }
        public Band Footer { get; set; }
    }
    public class Band
    {
        //public string Name { get; set; }
        public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();
    }
    public class ReportGenerator
    {
        private Report report;
        public ReportGenerator(Report report)
        {
            this.report = report;
        }
        public void Generate(string newFile, string templateName)
        {
            using (var newFilePackage = new ExcelPackage(new FileInfo(newFile), new FileInfo(templateName)))
            {
                newFilePackage.Save();
            }
            //using (var newfilePackage = new ExcelPackage(new FileInfo(newFile)))
            //{
            //    int rowNewFile = 1;
            //    var wbNewFile = newfilePackage.Workbook;
            //    var wshNewFile = wbNewFile.Worksheets.Add("Лист 1");
            //    using (var templatePackage = new ExcelPackage(new FileInfo(templateName)))
            //    {
            //        var wbTemplate = templatePackage.Workbook;
            //        var wshTemplate = wbTemplate.Worksheets[1];
                    
            //        var headerRange = templatePackage.Workbook.Names["Header"];
            //        headerRange.Copy(wshNewFile.Cells[rowNewFile, 1]);
            //        foreach(var cell in )
            //    }
            //    newfilePackage.Save();
            //}
            //var app = new Microsoft.Office.Interop.Excel.Application();
            //Workbook wbNewFile = app.Workbooks.Add();
            //Worksheet wshNewFile = wbNewFile.Worksheets[1];
            //Workbook wbTemplate = app.Workbooks.Open(Filename: templateName);
            //Worksheet wshTemplate = (Worksheet)wbTemplate.Worksheets[1];

            //int rowNewFile = 1;
            //wshTemplate.Range["Header"].Copy(Destination: wshNewFile.Range[rowNewFile, 1]);

            ////Dictionary<string, object> dict = report.Header.Data[0];
            ////foreach (Range cell in wshTemplate.Range["Header"])
            ////{

            ////    if (dict.ContainsKey(cell?.Value ?? ""))
            ////    {
            ////        cell.Value = dict[cell.Value];
            ////    }
            ////}
            //wbTemplate.Close();
            //wbNewFile.SaveAs(Filename: newFile);
            //wbNewFile.Close();
        }
    }
    public class PrintInvoice
    {
        public static void DoIt(Invoice invoice)
        {
            var report = new Report();

            var headerBand = new Band();
            var d = new Dictionary<string, object>();
            d.Add("OrganizationName", "Галиум");
            d.Add("Num", invoice.Id);
            d.Add("Dt", invoice.Dt);
            headerBand.Data.Add(d);

            report.Header = headerBand;
            var rg = new ReportGenerator(report);

            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "invoice.xlsx");
            var tmpName = Global.Source.GetTempFilename(".xlsx");
            //File.Copy(templateName, tmpName);
            rg.Generate(tmpName, templateName);

            Process prc = new Process();
            prc.StartInfo.Arguments = "\"" + tmpName + "\"";
            prc.StartInfo.FileName = "excel.exe";
            prc.Start();

            //if (ofs.Length == 0) return;
            //using (var ctx = new OfsContext())
            //{
            //    using (var package = new ExcelPackage())
            //    {
            //        var wsh = package.Workbook.Worksheets.Add("Лист1");

            //        wsh.Column(1).Width = 32.75;
            //        wsh.DefaultColWidth = 12;
            //        wsh.Column(1).Style.WrapText = true;
            //        wsh.Row(7).Style.WrapText = true;
            //        wsh.Row(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //        wsh.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //        wsh.Cells[1, 1].Value = "Оценка финансового состояния";
            //        wsh.Cells[1, 1].Style.Font.Bold = true;
            //        wsh.Cells[1, 1].Style.Font.Size = 14;
            //        wsh.Cells[3, 1].Value = "Организация";
            //        wsh.Cells[3, 2].Value = ctx.Clients.Find(ofs[0].Inn).Name;
            //        wsh.Cells[3, 2].Style.Font.Bold = true;
            //        wsh.Cells[4, 1].Value = "ИНН";
            //        wsh.Cells[4, 2].Value = ofs[0].Inn;
            //        wsh.Cells[4, 2].Style.Font.Bold = true;
            //        wsh.Cells[5, 1].Value = "Единица измерения (абсолютные показатели)";
            //        wsh.Cells[5, 2].Value = "тыс.руб.";
            //        wsh.Cells[5, 2].Style.Font.Bold = true;

            //        wsh.Cells[7, 1].Value = "Наименование показателей";
            //        wsh.Cells[8, 1].Value = "1";
            //        wsh.Cells[9, 1].Value = "Коэффициент финансовой независимости";
            //        wsh.Cells[10, 1].Value = "Коэффициент обеспеченности собственными оборотными средствами";
            //        wsh.Cells[11, 1].Value = "Коэффициент соотношения оборотных и внеоборотных активов";
            //        wsh.Cells[12, 1].Value = "Общий коэффициент ликвидности";
            //        wsh.Cells[13, 1].Value = "Коэффициент покрытия";
            //        wsh.Cells[14, 1].Value = "Коэффициент оборачиваемости активов";
            //        wsh.Cells[15, 1].Value = "Рентабельность продаж";
            //        wsh.Cells[16, 1].Value = "Рентабельность собственного капитала (чистых активов)";
            //        wsh.Cells[17, 1].Value = "Обобщающий результат";

            //        wsh.Cells[19, 1].Value = "Динамика отдельных показателей";
            //        wsh.Cells[19, 1].Style.Font.Bold = true;

            //        var blines = ctx.Blines.OrderBy(s => s.CodeSort).ToList();

            //        int row = 20;
            //        foreach (var bline in blines)
            //        {
            //            wsh.Cells[row, 1].Value = $"{bline.Name} ({bline.Code})";
            //            row++;
            //        }

            //        int j = 2;
            //        for (int i = 0; i < ofs.Length; i++)
            //        {
            //            wsh.Cells[7, j].Value = DateFromQuater(ofs[i].Year, ofs[i].Quater);
            //            wsh.Cells[7, j].Style.Numberformat.Format = "dd.MM.yyyy";
            //            wsh.Cells[7, j].Style.Font.Bold = true;
            //            wsh.Cells[8, j].Value = j.ToString();
            //            wsh.Cells[9, j].Value = ofs[i].Kfn;
            //            wsh.Cells[10, j].Value = ofs[i].Kosos;
            //            wsh.Cells[11, j].Value = ofs[i].Ksova;
            //            wsh.Cells[12, j].Value = ofs[i].Okl;
            //            wsh.Cells[13, j].Value = ofs[i].Kp;
            //            wsh.Cells[14, j].Value = ofs[i].Koa;
            //            wsh.Cells[15, j].Value = ofs[i].Rp;
            //            wsh.Cells[16, j].Value = ofs[i].Rsk;
            //            wsh.Cells[17, j].Value = ofs[i].getRop();

            //            row = 20;
            //            foreach (var bline in blines)
            //            {
            //                wsh.Cells[row, j].Value = ofs[i].Balance.FirstOrDefault(s => s.Code == bline.Code).Sm;
            //                row++;
            //            }
            //            j++;
            //            if (j == 3) continue;
            //            if (j == 4)
            //                wsh.Cells[7, j].Value = $"Изменения (гр.{j - 1} - гр.{j - 2})";
            //            else
            //                wsh.Cells[7, j].Value = $"Изменения (гр.{j - 1} - гр.{j - 4})";
            //            wsh.Cells[8, j].Value = j.ToString();
            //            wsh.Cells[9, j].Value = ofs[i].Kfn - ofs[i - 1].Kfn;
            //            wsh.Cells[10, j].Value = ofs[i].Kosos - ofs[i - 1].Kosos;
            //            wsh.Cells[11, j].Value = ofs[i].Ksova - ofs[i - 1].Ksova;
            //            wsh.Cells[12, j].Value = ofs[i].Okl - ofs[i - 1].Okl;
            //            wsh.Cells[13, j].Value = ofs[i].Kp - ofs[i - 1].Kp;
            //            wsh.Cells[14, j].Value = ofs[i].Koa - ofs[i - 1].Koa;
            //            wsh.Cells[15, j].Value = ofs[i].Rp - ofs[i - 1].Rp;
            //            wsh.Cells[16, j].Value = ofs[i].Rsk - ofs[i - 1].Rsk;
            //            wsh.Cells[17, j].Value = ofs[i].getRop() - ofs[i - 1].getRop();

            //            row = 20;
            //            foreach (var bline in blines)
            //            {
            //                wsh.Cells[row, j].Value = ofs[i].Balance.FirstOrDefault(s => s.Code == bline.Code).Sm -
            //                                            ofs[i - 1].Balance.FirstOrDefault(s => s.Code == bline.Code).Sm;
            //                row++;
            //            }
            //            j++;
            //            if (j == 5)
            //                wsh.Cells[7, j].Value = $"Изменения (гр.{j - 2} - гр.{j - 3}) %";
            //            else
            //                wsh.Cells[7, j].Value = $"Изменения (гр.{j - 2} - гр.{j - 5}) %";
            //            wsh.Cells[8, j].Value = j.ToString();
            //            try
            //            {
            //                wsh.Cells[9, j].Value = getPercent(ofs[i].Kfn, ofs[i - 1].Kfn);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[10, j].Value = getPercent(ofs[i].Kosos, ofs[i - 1].Kosos);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[11, j].Value = getPercent(ofs[i].Ksova, ofs[i - 1].Ksova);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[12, j].Value = getPercent(ofs[i].Okl, ofs[i - 1].Okl);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[13, j].Value = getPercent(ofs[i].Kp, ofs[i - 1].Kp);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[14, j].Value = getPercent(ofs[i].Koa, ofs[i - 1].Koa);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[15, j].Value = getPercent(ofs[i].Rp, ofs[i - 1].Rp);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[16, j].Value = getPercent(ofs[i].Rsk.Value, ofs[i - 1].Rsk.Value);
            //            }
            //            catch (DivideByZeroException) { }
            //            try
            //            {
            //                wsh.Cells[17, j].Value = getPercent(ofs[i].getRop(), ofs[i - 1].getRop());
            //            }
            //            catch (DivideByZeroException) { }

            //            row = 20;
            //            foreach (var bline in blines)
            //            {
            //                try
            //                {
            //                    wsh.Cells[row, j].Value = getPercent(ofs[i].Balance.FirstOrDefault(s => s.Code == bline.Code).Sm,
            //                        ofs[i - 1].Balance.FirstOrDefault(s => s.Code == bline.Code).Sm);
            //                }
            //                catch (DivideByZeroException) { }
            //                row++;
            //            }

            //            for (var row1 = 9; row1 <= row; row1++)
            //            {
            //                for (var col1 = j - 1; col1 <= j; col1++)
            //                {
            //                    try
            //                    {
            //                        if (Convert.ToDecimal(wsh.Cells[row1, col1].Value) > 0)
            //                        {
            //                            wsh.Cells[row1, col1].Style.Font.Color.SetColor(System.Drawing.Color.Blue);
            //                        }
            //                        else if (Convert.ToDecimal(wsh.Cells[row1, col1].Value) < 0)
            //                        {
            //                            wsh.Cells[row1, col1].Style.Font.Color.SetColor(System.Drawing.Color.Red);
            //                        }
            //                    }
            //                    catch { }
            //                }
            //            }

            //            j++;
            //        }
            //        //wsh.Cells[9, 2, wsh.Dimension.End.Row, wsh.Dimension.End.Column].Style.Font.Bold = true;
            //        wsh.Cells[7, 1, 17, wsh.Dimension.End.Column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[7, 1, 17, wsh.Dimension.End.Column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[7, 1, 17, wsh.Dimension.End.Column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[7, 1, 17, wsh.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[20, 1, row - 1, wsh.Dimension.End.Column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[20, 1, row - 1, wsh.Dimension.End.Column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[20, 1, row - 1, wsh.Dimension.End.Column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            //        wsh.Cells[20, 1, row - 1, wsh.Dimension.End.Column].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            //        package.File = new FileInfo(Path.Combine(Path.GetTempPath(), "__ofs__" + Guid.NewGuid().ToString() + ".xlsx"));
            //        package.Save();

            //        Process prc = new Process();
            //        prc.StartInfo.Arguments = "\"" + package.File + "\"";
            //        prc.StartInfo.FileName = "excel.exe";
            //        prc.Start();
            //    }
            //}
        }
    }
}
