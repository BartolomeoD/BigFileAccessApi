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
        private readonly List<Line> _indexedLines;

        public IndexerService(int indexDefaultBufferSize)
        {
            _defaultBufferSize = indexDefaultBufferSize;
            _indexedLines = new List<Line>();
        }

        public Line GetLine(int lineNumber)
        {
            return _indexedLines[lineNumber];
        }

        public void DeleteLine(int lineNumber, int lineLength)
        {
            _indexedLines.RemoveAt(lineNumber);
            for (var i = lineNumber; i < _indexedLines.Count; i++)
            {
                _indexedLines[i].StartOffset -= lineLength + 1;
            }
        }

        public void AppendLine(int lineLength)
        {
            var last = _indexedLines[_indexedLines.Count - 1];
            _indexedLines.Add(new Line
            {
                Length = lineLength,
                StartOffset = last.StartOffset + last.Length + 1
            });
        }

        public void AddLine(int lineNumber, int lineLength)
        {
            var current = _indexedLines[lineNumber];

            _indexedLines.Insert(lineNumber, new Line
            {
                StartOffset = current.StartOffset,
                Length = lineLength
            });

            for (var i = lineNumber + 1; i < _indexedLines.Count; i++)
            {
                _indexedLines[i].StartOffset += lineLength + 1;
            }
        }

        public void IndexFile(string path)
        {
            var buffer = new byte[_defaultBufferSize];
            var offset = 0;
            long prevNewLineOffset =  -1;
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
                        for (var i = 0; i < bufferSize; i++)
                        {
                            var a = (char) buffer[i];
                            if (buffer[i] == (byte) '\n')
                            {
                                newLineOffset =  offset + i;
                                _indexedLines.Add(new Line
                                {
                                    Length = (int) (newLineOffset - prevNewLineOffset - 1),
                                    StartOffset = prevNewLineOffset + 1
                                });
                                prevNewLineOffset = newLineOffset;
                            }
                        }
                        offset += bufferSize;
                    } while (offset < fileLength);
                }
            }
        }
    }
}