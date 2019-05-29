using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public partial class Specialty
    {
        public static int RootId(SaaMedEntities ctx, int specialtyId)
        {
            Specialty sp = ctx.Specialty.Find(specialtyId);
            while (sp.ParentId != null)
            {
                sp = ctx.Specialty.Find(sp.ParentId);
            }
            return sp.Id;
        }
    }
}
