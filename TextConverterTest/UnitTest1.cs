using Generator;
using TextConverterLibrary;
using TextConverterLibrary.Service.Extensions;
using Xunit;

namespace TextConverterTest
{
    public class UnitTest1
    {
        [Fact]
        public void ParallelJsonToXmlTest()
        {
            Format from = Format.JSON, to = Format.XML;
            var structure = Generate.AbstractSyntaxTree<Model>();

            var json = Generate.Text(structure, from);

            ConverterFactory.Method = ConverterFactory.Parallel;
            var converter = new Converter();

            var actualXml = converter.Convert(from:from, to:to, input:json);
            var expectedXml = Generate.Text(structure, to);

            var actual = converter.Deserialize(to, actualXml);
            var expected = converter.Deserialize(to, expectedXml);

            Assert.True(actual.IsEqual(expected));
        }
    }
}