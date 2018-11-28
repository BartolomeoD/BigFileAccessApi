namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileReader
    {
        string ReadLine(int lineNumber);
    }
}
