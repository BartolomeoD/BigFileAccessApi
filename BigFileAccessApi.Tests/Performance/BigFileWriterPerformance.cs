using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Services;
using NUnit.Framework;

namespace BigFileAccessApi.Tests.Performance
{
    [TestFixture]
    public class BigFileWriterPerformance
    {
        private BigFileWriter _writer;

        [SetUp]
        public void OpenBigFile()
        {
            File.Copy("default.txt", "big_file_test.txt");
            _writer = new BigFileWriter(new FileStream("big_file_test.txt", FileMode.Open));
        }

        [Test]
        public async Task Performance_AppendLineAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _writer.AppendLineAsync("aaa");
            stopwatch.Stop();
            Console.WriteLine($"Append line in {stopwatch.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task Performance_AddLineAsync()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _writer.AddLineAsync(100, "aaaaa");
            stopwatch.Stop();
            Console.WriteLine($"Added line in {stopwatch.ElapsedMilliseconds} ms");
        }

        [TearDown]
        public void Close()
        {
            _writer.Close();
            File.Delete("big_file_test.txt");
        }
    }
}