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

                var row = 6;
                // все направления
                foreach (var sp in ctx.Specialty.Where(s => s.ParentId == null).OrderBy(s => s.Name))
                {
                    wsh.Cells[row, 1].Value = sp.Name;
                    // услуги за день по выбранному направлению
                    foreach (var z1 in ctx.Zakaz1.Include(s => s.Zakaz).Include(s => s.Personal).Include(s => s.Benefit).Where(s => s.Zakaz.Dt == dt && s.Benefit.SpecialtyId == sp.Id))
                    {
                        if (
                    }
                    row++;
                }
                //foreach (var o in ctx.Zakaz1.Include(s => s.Zakaz).Include(s => s.Personal).Where(s => s.Zakaz.Dt == dt))
                //{
                //    //wsh.Cells[row++, 1].BorderAround().Value = o.Personal.
                //}

                pack.Save();
                return tmpName;
            }
        }
    }
}