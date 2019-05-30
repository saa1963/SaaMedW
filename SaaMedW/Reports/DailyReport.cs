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
                wsh.Column(1).Width = 23;
                wsh.Column(2).Width = 22;
                wsh.Column(3).Width = 9;
                wsh.Column(4).Width = 9;
                wsh.Column(5).Width = 9;
                wsh.Column(6).Width = 9;
                wsh.Cells[1, 1].Value = $"Отчет за оказанные мед.услуги от {dt.ToLongDateString()}";
                wsh.Cells[3, 1].Value = $"Медрегистратор {Global.Source.RUser.Fio}";
                wsh.Cells[5, 1].BorderAround().Value = "НАПРАВЛЕНИЕ";
                wsh.Cells[5, 2].BorderAround().WrapText().Value = "Ф.И.О. СПЕЦИАЛИСТА";
                wsh.Cells[5, 3].BorderAround().Value = "СУММА";
                wsh.Cells[5, 4].BorderAround().WrapText().Value = "в т.ч. ДМС";
                wsh.Cells[5, 5].BorderAround().Value = "ИТОГО";
                wsh.Cells[5, 6].BorderAround().WrapText().Value = "в т.ч. ДМС";

                decimal sm0 = 0;
                decimal sm0_dms = 0;
                var row = 6;
                // все направления
                foreach (var sp in ctx.Specialty.Where(s => s.ParentId == null).OrderBy(s => s.Name))
                {
                    var row0 = row;
                    decimal sm = 0;
                    decimal sm_dms = 0;
                    wsh.Cells[row, 1].WrapText().Value = sp.Name;
                    // услуги за день по выбранному направлению
                    foreach (var z1 in ctx.Zakaz1.Where(s => s.Zakaz.Dt == dt && s.SpecialtyRootId == sp.Id).GroupBy(s => s.Personal))
                    {
                        wsh.Cells[row, 2].BorderAround().WrapText().Value = z1.Key.Fio;
                        wsh.Cells[row, 3].BorderAround().Value = z1.Sum(s => s.Kol * s.Price);
                        wsh.Cells[row, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row, 4].BorderAround().Value = z1.Where(s => s.Zakaz.Dms).Sum(s => s.Kol * s.Price);
                        wsh.Cells[row, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sm += z1.Sum(s => s.Kol * s.Price);
                        sm_dms += z1.Where(s => s.Zakaz.Dms).Sum(s => s.Kol * s.Price);
                        row++;
                    }
                    if (row == row0)
                    {
                        wsh.Cells[row, 1].BorderAround();
                        wsh.Cells[row, 2].BorderAround();
                        wsh.Cells[row, 3].BorderAround();
                        wsh.Cells[row, 4].BorderAround();
                        wsh.Cells[row, 5].BorderAround();
                        wsh.Cells[row, 6].BorderAround();
                        row++;
                    }
                    else
                    {
                        wsh.Cells[row0, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row0, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row0, 5].Value = sm;
                        wsh.Cells[row0, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        wsh.Cells[row0, 6].Value = sm_dms;
                        wsh.Cells[row0, 1, row - 1, 1].BorderAround().Merge = true;
                        wsh.Cells[row0, 5, row - 1, 5].BorderAround().Merge = true;
                        wsh.Cells[row0, 6, row - 1, 6].BorderAround().Merge = true;
                        sm0 += sm;
                        sm0_dms += sm_dms;
                    }
                }
                wsh.Cells[row, 1].Value = "ВСЕГО:";
                wsh.Cells[row, 5].Value = sm0;
                wsh.Cells[row, 6].Value = sm0_dms;

                pack.Save();
                return tmpName;
            }
        }
    }
}