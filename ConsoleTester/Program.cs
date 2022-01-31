using PerformanceCompare;
using TextConverterLibrary;
using Generator;
using System;

namespace ConsoleTester
{
    public static class Program
    {
        public static void Main()
        {
            Performance.Compare(() => {

                ConverterFactory.Method = ConverterFactory.Parallel;
                var converter = new Converter();

                var json = Generate.Text(Generate.AbstractSyntaxTree<Model>(), Format.JSON);
                
                var xml = converter.Convert(from:Format.JSON, to:Format.XML, input:json);

                return "Parrallel json to xml conversion";
            }, () => {

                ConverterFactory.Method = ConverterFactory.Recursion;
                var converter = new Converter();

                var json = Generate.Text(Generate.AbstractSyntaxTree<Model>(), Format.JSON);

                var xml = converter.Convert(from: Format.JSON, to: Format.XML, input: json);

                return "Recusion json to xml conversion";
            }).Print(Console.WriteLine);
        }
    }
}