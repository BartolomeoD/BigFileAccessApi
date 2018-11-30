using System.Collections.Generic;
using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IIndexerService
    {
        List<Line> IndexFile(string path);
    }
}
