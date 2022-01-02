namespace TextConverterLibrary
{
    public interface IConverter
    {
        public string Convert(Format from, Format to, string input);
    }
}