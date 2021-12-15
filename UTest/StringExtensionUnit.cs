using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;

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
            
        }
    }
}