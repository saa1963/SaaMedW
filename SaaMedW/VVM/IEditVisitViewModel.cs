using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public interface IEditVisitViewModel
    {
        SaaMedEntities ctx { get; set; }
        Benefit BenefitSel { get; set; }
        int SelectedPersonId { get; set; }
    }
}
