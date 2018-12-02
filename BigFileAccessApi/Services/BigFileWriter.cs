using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Abstractions;

namespace BigFileAccessApi.Services
{
    public class BigFileWriter : IBigFileWriter
    {
        private readonly FileStream _fileStream;
        private const int DefaultBufferSize = 4096;

        public BigFileWriter(string filePath)
        {
            _fileStream = File.Open(filePath,FileMode.Open);
        }
        
        public async Task AddLineAsync(int position, string value)
        {
            Shift(position, value.Length);
            _fileStream.Seek(position, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(value + "\n");
            await _fileStream.WriteAsync(buffer);
        }

        public async Task AppendLineAsync(string value)
        {
            _fileStream.Seek(_fileStream.Length, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(value + "\n");
            await _fileStream.WriteAsync(buffer);
        }

        public void Close()
        {
            _fileStream.Close();
        }

        public void Shift(long fromPosition, int offsetPosition)
        {
            var buffers = new Queue<byte[]>();
            var initialLength = _fileStream.Length;
            var readOffset = fromPosition;
            var writeOffset = fromPosition + offsetPosition;
            do
            {
                if (initialLength > readOffset)
                {
                    int bufferSize;
                    if (readOffset + DefaultBufferSize > initialLength)
                        bufferSize = (int) (initialLength - readOffset);
                    else
                        bufferSize = DefaultBufferSize;
                    var buffer = new byte[bufferSize];
                    _fileStream.Seek(readOffset, SeekOrigin.Begin);
                    _fileStream.Read(buffer);
                    buffers.Enqueue(buffer);
                    readOffset += bufferSize;
                }

                if (readOffset > fromPosition + offsetPosition + 1)
                {
                    _fileStream.Seek(writeOffset, SeekOrigin.Begin);
                    var writingBytes = buffers.Dequeue();
                    _fileStream.Write(writingBytes);
                    writeOffset += writingBytes.Length;
                }

                if (buffers.Count == 0)
                    break;

            } while (true);
        }
    }
}
