namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileWriter
    {
        void AddLineAsync(int position, string value);

        void AppendLineAsync(string value);
    }
}
