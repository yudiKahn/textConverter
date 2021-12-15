using ClassLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTest
{
    [TestClass]
    public class ConversionUnit
    {
        private string jsonA = " [ \"first string value of array\", \"person\": { \"name\": \"dani\" } ] ";
        private string jsonB = " { \"objectName\": \"my object\", \"users\": [ \"dani\", \" levi\" ]   } ";
        [TestMethod]
        public void TestToXml()
        {
        }
    }
}
