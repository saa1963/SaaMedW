using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public static class ExcelRangeExtentions
    {
        public static ExcelRange BorderAround(this ExcelRange r)
        {
            r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            return r;
        }

        public static void AllBorderAround(this ExcelRange r)
        {
            foreach (var o in r)
            {
                o.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                o.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                o.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                o.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }
        }
    }
}
