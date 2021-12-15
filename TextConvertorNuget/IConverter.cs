namespace TextConvertorNuget
{
    public interface IConverter
    {
        public Func<string, string> this[(Format, Format) formats] { get; }
    }
}