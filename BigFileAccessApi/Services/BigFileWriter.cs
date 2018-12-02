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

        public BigFileWriter(FileStream fileStream)
        {
            _fileStream = fileStream;
        }
        
        public async Task AddLineAsync(long position, string value)
        {
            var insertingValue = value + "\n";
            Shift(position, insertingValue.Length);
            _fileStream.Seek(position, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(insertingValue);
            await _fileStream.WriteAsync(buffer);
        }

        public async Task AppendLineAsync(string value)
        {
            _fileStream.Seek(_fileStream.Length, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(value + "\n");
            await _fileStream.WriteAsync(buffer);
        }

        public async Task DeleteLineAsync(long position, int length)
        {
            var readOffset = position + length + 1;
            var writeOffset = position;
            var buffer = new byte[DefaultBufferSize];
            do
            {
                int bufferSize;
                if (readOffset + DefaultBufferSize > _fileStream.Length)
                    bufferSize = (int) (_fileStream.Length - readOffset);
                else
                    bufferSize = DefaultBufferSize;
                _fileStream.Seek(readOffset, SeekOrigin.Begin);
                await _fileStream.ReadAsync(buffer, 0, bufferSize);
                _fileStream.Seek(writeOffset, SeekOrigin.Begin);
                await _fileStream.WriteAsync(buffer, 0, bufferSize);
                readOffset += bufferSize;
                writeOffset += bufferSize;
            } while (readOffset < _fileStream.Length);
            _fileStream.SetLength(writeOffset);
        }

        public void Close()
        {
            _fileStream.Close();
        }

        private void Shift(long fromPosition, int offsetPosition)
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

                if (readOffset > writeOffset || readOffset == initialLength)
                {
                    _fileStream.Seek(writeOffset, SeekOrigin.Begin);
                    var writingBytes = buffers.Dequeue();
                    _fileStream.Write(writingBytes);
                    writeOffset += writingBytes.Length;
                }
            } while (buffers.Count > 0);
        }
    }
}
