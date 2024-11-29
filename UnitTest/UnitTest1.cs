using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string ss = Core.Comm.Function.ConvertToChinese(109101231011.232);
            var ll = Core.Comm.Function.ConverToDate("02");
           string bb = ss;
        }
    }
}
