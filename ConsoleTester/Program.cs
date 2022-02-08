using TextConverterLibrary;
using Generator;

namespace ConsoleTester
{
    public static class Program
    {
        public static void Main()
        {
            ConverterFactory.Method = ConverterFactory.Recursion;
            var converter = new Converter();

            var json = Generate.Text(Generate.AbstractSyntaxTree<Model>(), Format.JSON);

            var xml = converter.Convert(from: Format.JSON, to: Format.XML, input: json);

            Console.WriteLine(xml);
        }
    }
}