using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class Print
    {
        public static void PrintDogovor(DateTime dt, int num, Person person, bool isopenprint)
        {
            var fname = new Dogovor().GetDogovorFname(dt, num, person);
            if (!isopenprint)
            {
                HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public static void PrintVmesh(DateTime dt, Person person, bool isopenprint)
        {
            var fname = new Vmesh().DoIt(dt, person);
            if (!isopenprint)
            {
                HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public static void PrintMedcard(Person person, bool isopenprint)
        {
            var fname = new MedCard().DoIt(person);
            if (!isopenprint)
            {
                HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public static void ZakazReport(DateTime dt, int num, Person person, 
            bool dms, IEnumerable<BenefitForZakaz> lst, bool isopenprint)
        {
            var fname = new ZakazReport().DoIt(dt, num, person, dms, lst);
            if (!isopenprint)
            {
                HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        private static void HardCopy(string fname)
        {
            ProcessStartInfo info = new ProcessStartInfo(fname);
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(info);
            process.WaitForExit(10000);
        }
    }
}
