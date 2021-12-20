using Serilog;
using Interfaces = TextConvertorNuget.Interfaces;

namespace TextConvertorNuget
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

        public Converter(
            ILogger logger,
            Interfaces.IJsonDeserializer jsonDeserializer, Interfaces.IJsonSerializer jsonSerializer, 
            Interfaces.IXmlDeserializer xmlDeserializer, Interfaces.IXmlSerializer xmlSerializer)
        {
            _logger = logger;
            _json_deserializer = jsonDeserializer;
            _json_serializer = jsonSerializer;
            _xml_serializer = xmlSerializer;
            _xml_deserializer = xmlDeserializer;

            _dict = new()
            {
                { (Format.JSON, Format.XML), Convert(_json_deserializer, _xml_serializer) },
                { (Format.XML, Format.JSON), Convert(_xml_deserializer, _json_serializer) },
                { (Format.XML, Format.XML), Convert(_xml_deserializer, _xml_serializer) },
                { (Format.JSON, Format.JSON), Convert(_json_deserializer, _json_serializer) },
            };
        }

        public Func<string, string> this[(Format,Format) tuple]
        {
            get
            {
                return _dict[(tuple.Item1, tuple.Item2)];
            }
        }
        private Func<string, string> Convert(IDeserializer deserializer, ISerializer serializer) => (input) =>
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