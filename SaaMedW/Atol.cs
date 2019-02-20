using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atol.Drivers10.Fptr;

namespace SaaMedW
{
    public class Atol
    {
        private IFptr fptr;
        public Atol(IFptr _fptr)
        {
            fptr = _fptr;
        }
        public string ShowProperties()
        {
            int res = fptr.showProperties(Constants.LIBFPTR_GUI_PARENT_NATIVE, (IntPtr)0);
            if (res == 0)
                return fptr.getSettings();
            else
                return null;
        }
    }
}
