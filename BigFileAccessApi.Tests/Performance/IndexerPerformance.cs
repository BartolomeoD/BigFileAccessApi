using System;
using System.Diagnostics;
using BigFileAccessApi.Services;
using NUnit.Framework;

namespace BigFileAccessApi.Tests.Performance
{
    [TestFixture]
    public class IndexerPerformance
    {
        private IndexerService _indexer;

        private void IndexBigFile()
        {
            _indexer = new IndexerService(1000);
            _indexer.IndexFile(@"big_file.txt");
        }

        [Test]
        public void Performance_GetLine()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IndexBigFile();
            stopwatch.Stop();
            Console.WriteLine($"indexed in {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            _indexer.GetLine(500);
            stopwatch.Stop();
            Console.WriteLine($"500 line got in {stopwatch.ElapsedMilliseconds} ms");
        }

        [Test]
        public void Performance_AppendLine()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IndexBigFile();
            stopwatch.Stop();
            Console.WriteLine($"indexed in {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            _indexer.AppendLine(50);
            stopwatch.Stop();
            Console.WriteLine($"append line in {stopwatch.ElapsedMilliseconds} ms");
        }

        [Test]
        public void Performance_AddLine()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IndexBigFile();
            stopwatch.Stop();
            Console.WriteLine($"indexed in {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
            stopwatch.Start();
            _indexer.AddLine(100, 20);
            stopwatch.Stop();
            Console.WriteLine($"line added in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
