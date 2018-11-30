using System.Threading.Tasks;
using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IBigFileReader
    {
        Task<string> ReadLineAsync(Line line);
    }
}
