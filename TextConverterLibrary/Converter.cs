using Serilog;
using TextConverterLibrary.Interfaces;
using Interfaces = TextConverterLibrary.Interfaces;

namespace TextConverterLibrary
{
    public class Converter : IConverter
    {
        public const string RECURSION = "Recursion";
        public const string PARALLEL = "Parallel";

        private Interfaces.IJsonDeserializer _json_deserializer;
        private Interfaces.IXmlSerializer _xml_serializer;

        private Interfaces.IJsonSerializer _json_serializer;
        private Interfaces.IXmlDeserializer _xml_deserializer;
        
        private ILogger _logger;

        private Dictionary<(Format, Format), Func<string, string>> _dict;

        public Converter()
        {
            _logger = Factory.GetOfType<ILogger>();
            _json_deserializer = Factory.GetOfType<IJsonDeserializer>();
            _json_serializer = Factory.GetOfType<IJsonSerializer>();
            _xml_serializer = Factory.GetOfType<IXmlSerializer>();
            _xml_deserializer = Factory.GetOfType<IXmlDeserializer>();

            _dict = new()
            {
                { (Format.JSON, Format.XML), GetConverter(_json_deserializer, _xml_serializer) },
                { (Format.XML, Format.JSON), GetConverter(_xml_deserializer, _json_serializer) },
                { (Format.XML, Format.XML), GetConverter(_xml_deserializer, _xml_serializer) },
                { (Format.JSON, Format.JSON), GetConverter(_json_deserializer, _json_serializer) },
            };
        }
        public string Convert(Format from, Format to, string input) => _dict[(from, to)](input);

        private Func<string, string> GetConverter(IDeserializer deserializer, ISerializer serializer) => (input) =>
        {
            try
            {
                _logger.Information("Starting the process of parsing...");
                var nodes = deserializer.Deserialize(input);

                _logger.Information("Starting the process of converting...");
                string res = serializer.Serialize(nodes);

                _logger.Information("Finished successfully !!!");
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                throw;
            }
        };
    }
}