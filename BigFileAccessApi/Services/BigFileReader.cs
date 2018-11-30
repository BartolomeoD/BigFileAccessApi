using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Contracts;
using Microsoft.Extensions.Configuration;

namespace BigFileAccessApi.Services
{
    public class BigFileReader : IBigFileReader
    {
        private readonly FileStream _fileReader;
        public BigFileReader(IConfiguration appConfiguration)
        {
            _fileReader = File.OpenRead(appConfiguration["App:BigFilePath"]);
        }

        public async Task<string> ReadLineAsync(Line line)
        {
            _fileReader.Seek(line.StartOffset, SeekOrigin.Begin);
            var bytes = new byte[line.Length];
            await _fileReader.ReadAsync(bytes,0, line.Length);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
