using System.Threading.Tasks;

namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileWriter
    {
        Task AddLineAsync(long position, string value);

        Task AppendLineAsync(string value);

        Task DeleteLineAsync(long position, int length);

        void Close();
    }
}
