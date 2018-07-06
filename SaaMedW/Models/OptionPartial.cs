using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public partial class Options
    {
        public static string GetString(enumParameterType type, int userId)
        {
            using (var ctx = new SaaMedEntities())
            {
                var opt = ctx.Options.Find(new object[] { type, userId });
                Debug.Assert(opt != null);
                Debug.Assert(opt.DataType == "System.String");
                return opt.ParameterValue;
            }
        }
    }
}
