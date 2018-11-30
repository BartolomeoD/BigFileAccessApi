using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IIndexerService
    {
        Line[] IndexFile(string file);
    }
}
