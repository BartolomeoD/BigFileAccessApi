namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileWriter
    {
        void AddLine(int position, string value);

        void AppendLine(string value);
    }
}
