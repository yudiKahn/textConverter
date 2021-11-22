using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;
using StringExtension;

namespace UTest
{
    [TestClass]
    public class StringExtensionUnit
    {
        private string json = " { \"name\":\"yudi kahn\" } ";
        private string xml =  " <xml> <Users> <User>yudi kahn</User> </Users> </xml> ";
        [TestMethod]
        public void TestStringIsJson()
        {
            bool actual = json.isJson();
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestStringIsXml()
        {
            bool actual = xml.isXml();
            bool expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestRemoveAllWhiteSpace()
        {
            string jsonActual = json.RemoveAllWhiteSpace();
            string jsonExpected = "{\"name\":\"yudi kahn\"}";
            Assert.AreEqual(jsonExpected, jsonActual);

            string xmlActual = xml.RemoveAllWhiteSpace();
            string xmlExpected = "<xml><Users><User>yudi kahn</User></Users></xml>";
            Assert.AreEqual(xmlExpected, xmlActual);
        }
    }
}