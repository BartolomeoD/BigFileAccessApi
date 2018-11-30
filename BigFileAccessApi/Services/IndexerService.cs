using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Contracts;
using Microsoft.Extensions.Configuration;

namespace BigFileAccessApi.Services
{
    public class IndexerService : IIndexerService
    {
        private readonly int _defaultBufferSize;

        public IndexerService(IConfiguration configuration)
        {
            if (!Int32.TryParse(configuration["App:IndexDefaultBufferSize"], out _defaultBufferSize))
                throw new Exception("Не правильно указано значение App:MbPerIndexIteration");
        }

        public List<Line> IndexFile(string path)
        {
            var result = new List<Line>();
            var buffer = new byte[_defaultBufferSize];
            var offset = 0;
            long currentStringOffset = 0;
            long newLineOffset = 0;
            var currentStringNumber = 0;
            var fileLength = new FileInfo(path).Length;
            using (var file = MemoryMappedFile.CreateFromFile(path))
            {
                using (var accessor = file.CreateViewAccessor(0, fileLength))
                {
                    do
                    {
                        int bufferSize;
                        if (offset + _defaultBufferSize > fileLength)
                            bufferSize = (int) (fileLength - offset);
                        else
                            bufferSize = _defaultBufferSize;
                        accessor.ReadArray(offset, buffer, 0, bufferSize);
                        offset += bufferSize;
                        var prevNewLineOffset = 0;
                        var found = false;
                        for (var i = 0; i < bufferSize; i++)
                        {
                            if (buffer[i] == (byte) '\n')
                            {
                                found = true;
                                newLineOffset += i - prevNewLineOffset;
                                result.Add(new Line
                                {
                                    Number = currentStringNumber,
                                    Length = (int) (newLineOffset - currentStringOffset),
                                    StartOffset = currentStringOffset
                                });
                                currentStringNumber++;
                                currentStringOffset = newLineOffset + 1;
                                prevNewLineOffset = i;
                            }
                        }
                        if (!found)
                            newLineOffset += bufferSize;
                    } while (offset < fileLength);
                }
            }
            return result;
        }
    }
}