using System.IO.MemoryMappedFiles;
using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IIndexerService
    {
        Chunk[] IndexFile(MemoryMappedFile  file);
    }
}
