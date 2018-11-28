using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IIndexerService
    {
        Chunk[] IndexFile(string path);
    }
}
