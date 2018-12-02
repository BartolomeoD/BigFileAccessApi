using System;
using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Services;
using FluentAssertions;
using NUnit.Framework;

namespace BigFileAccessApi.Tests
{
    [TestFixture]
    public class BigFileWriterShould
    {
        private BigFileWriter _writer;
        private string id;

        [SetUp]
        public void IndexDefaultFile()
        {
            id = Guid.NewGuid().ToString();
            File.Copy("default.txt", $"default_{id}.txt");
            _writer = new BigFileWriter($"default_{id}.txt");
        }

        [Test]
        public async Task AppendLineToEnd()
        {
            await _writer.AppendLineAsync("123");
            _writer.Close();
            var lines = File.ReadAllLines($"default_{id}.txt");
            lines[3].Should().Be("123");
        }

        [Test]
        public async Task AddLineInMiddle()
        {
            await _writer.AddLineAsync(6, "aaa");
            _writer.Close();
            var lines = File.ReadAllLines($"default_{id}.txt");
            lines[1].Should().Be("aaa");
        }

        [TearDown]
        public void RemoveFile()
        {
            File.Delete($"default_{id}.txt");
        }
    }
}
