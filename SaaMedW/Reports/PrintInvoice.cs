using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Packaging;
using OfficeOpenXml.Style;

namespace SaaMedW
{
    public class MergedRange
    {
        public ExcelRange Range { get; set; }
        public double OldWidth;
    }
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
            using (var pack = new ExcelPackage(new FileInfo(newFile), new FileInfo(templateName)))
            {
                var wshSource = pack.Workbook.Worksheets[1];
                var wshDest = pack.Workbook.Worksheets.Add("Report");

                //ширина колонок
                for (int col = wshSource.Dimension.Start.Column; col <= wshSource.Dimension.End.Column; col++)
                {
                    wshDest.Column(col).Width = wshSource.Column(col).Width;
                }

                // Header
                var rowDest = 1;
                ExcelRangeBase destRange = wshDest.Cells[rowDest, 1];
                var band = pack.Workbook.Names["Header"];
                Dictionary<string, object> dict = report.Header.Data[0];
                BandCopy(wshSource, wshDest, band, destRange, dict);
                rowDest += band.Rows;
                destRange = wshDest.Cells[rowDest, 1];

                // Detail
                band = pack.Workbook.Names["Detail"];
                foreach (var d in report.Detail.Data)
                {
                    BandCopy(wshSource, wshDest, band, destRange, d);
                    rowDest += band.Rows;
                    destRange = wshDest.Cells[rowDest, 1];
                }

                // Footer
                band = pack.Workbook.Names["Footer"];
                dict = report.Footer.Data[0];
                BandCopy(wshSource, wshDest, band, destRange, dict);

                pack.Workbook.Worksheets.Delete(wshSource);
                pack.Save();
            }
        }

        private void BandCopy(ExcelWorksheet wshSource, ExcelWorksheet wshDest, ExcelNamedRange band, ExcelRangeBase destRange, Dictionary<string, object> dict)
        {
            List<MergedRange> ListOfMerges = null;
            band.Copy(destRange);
            destRange = destRange.Offset(0, 0, band.Rows, band.Columns);

            for (int row = destRange.Start.Row; row <= destRange.End.Row; row++)
            {
                for (int col = destRange.Start.Column; col <= destRange.End.Column; col++)
                {
                    var cell = wshDest.Cells[row, col];
                    if (dict.ContainsKey(cell.Text))
                    {
                        bool isMerged = false;
                        if (cell.Merge)
                        {
                            // здесь надо убрать все объединения в строке
                            isMerged = true;
                            ListOfMerges = RemoveMerges(wshDest, row);
                        }
                        cell.Value = dict[cell.Text];
                        if (isMerged)
                        {
                            // восстановить объединения в строке
                            RestoreMerges(wshDest, ListOfMerges);
                        }
                    }
                    //MergeArea = null;
                    //if (GetMergeArea(cell, out MergeArea)) // если ячейка объединена и левая верхняя в объединении
                    //{
                    //    //mx0 = MeasureTextHeight(cell.Text, cell.Style.Font, Convert.ToInt32(widthMergeArea));
                    //    mx0 = MeasureTextHeight();
                    //    if (mx0 > mx) mx = mx0;
                    //}
                }
                //if (mx != -1)
                //    wshDest.Row(row).Height = mx;
            }
        }

        private void RestoreMerges(ExcelWorksheet wshDest, List<MergedRange> listOfMerges)
        {
            foreach (var r in listOfMerges)
            {
                wshDest.Column(r.Range.Start.Column).Width = r.OldWidth;
                r.Range.Merge = true;
            }
        }

        private List<MergedRange> RemoveMerges(ExcelWorksheet wshDest, int row)
        {
            var rt = GetMergeAreas(wshDest, wshDest.Row(row));
            foreach (var r in rt)
            {
                r.Range.Merge = false;
                double smWidth = 0;
                for (int col = r.Range.Start.Column; col <= r.Range.End.Column; col++)
                {
                    smWidth += wshDest.Column(col).Width;
                }
                wshDest.Column(r.Range.Start.Column).Width = smWidth;
            }
            return rt;
        }

        private List<MergedRange> GetMergeAreas(ExcelWorksheet wsh, ExcelRow rc)
        {
            var rt = new List<MergedRange>();
            foreach (string r in wsh.MergedCells)
            {
                if (r != null)
                {
                    var rg = wsh.Cells[r];
                    if (rg.Start.Row == rc.Row && rg.End.Row == rc.Row)
                    {
                        rt.Add(new MergedRange() { Range = rg, OldWidth = wsh.Column(rg.Start.Column).Width });
                    }
                }
            }
            return rt;
        }
    }
    public class PrintInvoice
    {
        public static void DoIt(Invoice invoice)
        {
            var report = new Report();

            var headerBand = new Band();
            var d = new Dictionary<string, object>();
            d.Add("${OrganizationName}", "Галиум");
            d.Add("${NumDt}", "Счет № " + invoice.Id.ToString() + " от " + invoice.Dt.ToString("dd.MM.yyyy"));
            headerBand.Data.Add(d);
            report.Header = headerBand;

            var detailBand = new Band();
            int nn = 1; decimal sm = 0;
            foreach (var detail in invoice.InvoiceDetail)
            {
                d = new Dictionary<string, object>();
                d.Add("${Nn}", nn);
                d.Add("${BenefitName}", detail.BenefitName);
                d.Add("${Kol}", detail.Kol);
                d.Add("${Price}", detail.Price);
                d.Add("${Sm}", detail.Sm);
                detailBand.Data.Add(d);
                sm += detail.Sm;
                nn++;
            }
            report.Detail = detailBand;

            var footerBand = new Band();
            d = new Dictionary<string, object>();
            d.Add("${Itogo}", sm);
            footerBand.Data.Add(d);
            report.Footer = footerBand;

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
        }
    }
}
