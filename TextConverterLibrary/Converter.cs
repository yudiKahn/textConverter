using Serilog;
using TextConverterLibrary.Interfaces;

namespace TextConverterLibrary
{
    public class Converter : IConverter
    {
        private IJsonDeserializer _json_deserializer;
        private IXmlSerializer _xml_serializer;

        private IJsonSerializer _json_serializer;
        private IXmlDeserializer _xml_deserializer;
        
        private ILogger _logger;

        public IEnumerable<NodeAbstraction> Deserialize(Format from, string input) => 
            from == Format.JSON ? 
            _json_deserializer.Deserialize(input) : _xml_deserializer.Deserialize(input);


        private Dictionary<(Format, Format), Func<string, string>> _dict;

        public Converter()
        {
            _logger = ConverterFactory.GetOfType<ILogger>();
            _json_deserializer = ConverterFactory.GetOfType<IJsonDeserializer>();
            _json_serializer = ConverterFactory.GetOfType<IJsonSerializer>();
            _xml_serializer = ConverterFactory.GetOfType<IXmlSerializer>();
            _xml_deserializer = ConverterFactory.GetOfType<IXmlDeserializer>();

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