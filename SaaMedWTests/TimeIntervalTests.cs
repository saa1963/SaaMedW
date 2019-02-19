using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaaMedW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Tests
{
    [TestClass()]
    public class TimeIntervalTests
    {
        [TestMethod()]
        public void NextTest()
        {
            var tm1 = new TimeInterval()
            {
                Begin = DateTime.Now, Interval = new TimeSpan(0, 10, 0)
            };
            var tm2 = tm1.Next();
            var e = tm2.Begin - tm1.End;
            if (e.Ticks != 0)
                Assert.Fail("Ошибочка");
        }
    }
}