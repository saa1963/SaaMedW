using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class Months
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Name
        {
            get => Year.ToString() + " " + Global.Source.GetNameOfMonth0(Month);
        }
    }
}
