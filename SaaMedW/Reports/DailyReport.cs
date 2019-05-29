using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Data.Entity;
using System.Linq;

namespace SaaMedW
{
    internal class DailyReport
    {
        public DailyReport()
        {
        }

        internal string DoIt(DateTime dt)
        {
            var ctx = new SaaMedEntities();
            var tmpName = Global.Source.GetTempFilename(".xlsx");
            using (var pack = new ExcelPackage(new FileInfo(tmpName)))
            {
                var wsh = pack.Workbook.Worksheets.Add("Report");
                wsh.Column(1).Width = 34;
                wsh.Column(2).Width = 22;
                wsh.Column(3).Width = 12;
                wsh.Column(4).Width = 12;
                wsh.Cells[1, 1].Value = $"Отчет за оказанные мед.услуги от {dt.ToLongDateString()}";
                wsh.Cells[3, 1].Value = $"Медрегистратор {Global.Source.RUser.Fio}";
                wsh.Cells[5, 1].BorderAround().Value = "НАПРАВЛЕНИЕ";
                wsh.Cells[5, 2].BorderAround().Value = "Ф.И.О. СПЕЦИАЛИСТА";
                wsh.Cells[5, 3].BorderAround().Value = "СУММА";
                wsh.Cells[5, 4].BorderAround().Value = "ИТОГО";

                decimal sm0 = 0;
                var row = 6;
                // все направления
                foreach (var sp in ctx.Specialty.Where(s => s.ParentId == null).OrderBy(s => s.Name))
                {
                    var row0 = row;
                    decimal sm = 0;
                    wsh.Cells[row, 1].WrapText().Value = sp.Name;
                    //var lst = ctx.Zakaz1.Where(s => s.Zakaz.Dt == dt).GroupBy(s => s.Personal).ToList();
                    // услуги за день по выбранному направлению
                    foreach (var z1 in ctx.Zakaz1.Where(s => s.Zakaz.Dt == dt && s.SpecialtyRootId == sp.Id).GroupBy(s => s.Personal))
                    {
                        wsh.Cells[row, 2].BorderAround().WrapText().Value = z1.Key.Fio;
                        wsh.Cells[row, 3].BorderAround().Value = z1.Sum(s => s.Kol * s.Price);
                        sm += z1.Sum(s => s.Kol * s.Price);
                        row++;
                    }
                    if (row == row0)
                    {
                        wsh.Cells[row, 1].BorderAround();
                        wsh.Cells[row, 2].BorderAround();
                        wsh.Cells[row, 3].BorderAround();
                        wsh.Cells[row, 4].BorderAround();
                        row++;
                    }
                    else
                    {
                        wsh.Cells[row0, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row0, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row0, 4].Value = sm;
                        wsh.Cells[row0, 1, row - 1, 1].BorderAround().Merge = true;
                        wsh.Cells[row0, 4, row - 1, 4].BorderAround().Merge = true;
                        sm0 += sm;
                    }
                }
                wsh.Cells[row, 1].Value = "ВСЕГО:";
                wsh.Cells[row, 4].Value = sm0;
                //wsh.Cells[6, 1, row, 4].AllBorderAround();

                pack.Save();
                return tmpName;
            }
        }
    }
}