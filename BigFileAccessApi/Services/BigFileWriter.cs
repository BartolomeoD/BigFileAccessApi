using System.IO;
using BigFileAccessApi.Abstractions;
using Microsoft.Extensions.Configuration;

namespace BigFileAccessApi.Services
{
    public class BigFileWriter : IBigFileWriter
    {
        private FileStream _fileWriter;
        
        public BigFileWriter(IConfiguration appConfiguration)
        {
            _fileWriter = File.OpenWrite(appConfiguration["App:BigFilePath"]);
        }
        
        public async void AddLineAsync(int position, string value)
        {
            _fileWriter.Seek(position, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(value);
            await _fileWriter.WriteAsync(buffer);
        }

        public async void AppendLineAsync(string value)
        {
            _fileWriter.Seek(_fileWriter.Length, SeekOrigin.Begin);
            var buffer = System.Text.Encoding.UTF8.GetBytes(value);
            await _fileWriter.WriteAsync(buffer);
        }
    }
}
