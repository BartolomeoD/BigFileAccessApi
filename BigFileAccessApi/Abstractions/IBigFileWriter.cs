using System.Threading.Tasks;

namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileWriter
    {
        Task AddLineAsync(int position, string value);

        Task AppendLineAsync(string value);
    }
}
