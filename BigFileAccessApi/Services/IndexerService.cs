using System;
using System.IO.MemoryMappedFiles;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Contracts;
using Microsoft.Extensions.Configuration;
using static System.Int32;

namespace BigFileAccessApi.Services
{
    public class IndexerService : IIndexerService
    {
        private readonly int _mbPerIndexOperation;
        private readonly int _linesPerChunk;

        public IndexerService(IConfiguration configuration)
        {
            if (!TryParse(configuration["App:MbPerIndexIteration"], out _mbPerIndexOperation))
                throw new Exception("Не правильно указано значение App:MbPerIndexIteration");
            if (!TryParse(configuration["App:LinesPerChunk"], out _linesPerChunk))
                throw new Exception("Не правильно указано значение App:LinesPerChunk");
        }

        public Chunk[] IndexFile(MemoryMappedFile file)
        {
            using (var accessor = file.CreateViewAccessor())
            {
                var fileOfset = 0;
                while (true)
                {
                    accessor.ReadArray(1, new byte[100], 0, 100);
                }
            }
        }
    }
}
