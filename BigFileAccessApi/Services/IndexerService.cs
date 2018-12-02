using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Services
{
    public class IndexerService : IIndexerService
    {
        private readonly int _defaultBufferSize;
        private readonly List<Line> indexedLines;

        public IndexerService(int indexDefaultBufferSize)
        {
            _defaultBufferSize = indexDefaultBufferSize;
            indexedLines = new List<Line>();
        }

        public Line GetLine(int lineNumber)
        {
            return indexedLines[lineNumber];
        }

        public void AppendLine(int lineLength)
        {
            var last = indexedLines[indexedLines.Count - 1];
            indexedLines.Add(new Line
            {
                Length = lineLength,
                StartOffset = last.StartOffset + last.Length + 1
            });
        }

        public void AddLine(int lineNumber, int lineLength)
        {
            var preview = indexedLines[lineNumber - 1];


            indexedLines.Insert(lineNumber, new Line
            {
                StartOffset = preview.StartOffset + preview.Length + 1,
                Length = lineLength
            });
        }

        public void IndexFile(string path)
        {
            var buffer = new byte[_defaultBufferSize];
            var offset = 0;
            long currentStringOffset = 0;
            long newLineOffset = 0;
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
                            var a = (char) buffer[i];
                            if (buffer[i] == (byte) '\n')
                            {
                                found = true;
                                newLineOffset += i - prevNewLineOffset;
                                indexedLines.Add(new Line
                                {
                                    Length = (int) (newLineOffset - currentStringOffset - 1),
                                    StartOffset = currentStringOffset
                                });
                                currentStringOffset = newLineOffset + 1;
                                prevNewLineOffset = i;
                            }
                        }

                        if (!found)
                            newLineOffset += bufferSize;
                    } while (offset < fileLength);
                }
            }
        }
    }
}