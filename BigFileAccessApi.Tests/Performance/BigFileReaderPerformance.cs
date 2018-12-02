using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Contracts;
using BigFileAccessApi.Services;
using NUnit.Framework;

namespace BigFileAccessApi.Tests.Performance
{
    public class BigFileReaderPerformance
    {
        private BigFileReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new BigFileReader(new FileStream("default.txt", FileMode.Open));
        }

        [Test]
        public async Task Performance_ReadLine()
        {
            var line = new Line
            {
                StartOffset = 100500,
                Length = 100
            };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var str = await _reader.ReadLineAsync(line);
            stopwatch.Stop();
            Console.WriteLine($"Read line in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
