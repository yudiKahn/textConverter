using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextConvertorNuget;

namespace MSTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestParallelJsonToXml()
        {
            Factory.method = Converter.PARALLEL;
            var converter = Factory.GetOfType<IConverter>();
            string jsonStr = @"{
                ""name"":""dani""
            }";
            string actual = converter[(Format.JSON, Format.XML)](jsonStr);
            string expected = @"<__root><name>dani</name></__root>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestParallelXmlToJson()
        {
            Factory.method= Converter.PARALLEL;
            var converter = Factory.GetOfType<IConverter>();
            string xmlStr = @"<person>
                <name>dani</name>
            </person>";
            string actual = converter[(Format.XML, Format.JSON)](xmlStr);
            string expected = @"{""person"":{""name"":""dani""}}";
            Assert.AreEqual(expected,actual);
        }
    }
}