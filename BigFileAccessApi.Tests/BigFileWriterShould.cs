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
        private string _id;

        [SetUp]
        public void IndexDefaultFile()
        {
            _id = Guid.NewGuid().ToString();
            File.Copy("default.txt", $"default_{_id}.txt");
            _writer = new BigFileWriter(new FileStream($"default_{_id}.txt", FileMode.Open));
        }

        [Test]
        public async Task AppendLineToEnd()
        {
            await _writer.AppendLineAsync("123");
            _writer.Close();
            var lines = File.ReadAllLines($"default_{_id}.txt");
            lines[3].Should().Be("123");
        }

        [Test]
        public async Task AddLineInMiddle()
        {
            await _writer.AddLineAsync(6, "aaa");
            _writer.Close();
            var lines = File.ReadAllLines($"default_{_id}.txt");
            lines[1].Should().Be("aaa");
        }

        [Test]
        public async Task DeleteLine()
        {
            await _writer.DeleteLineAsync(6, 3);
            _writer.Close();
            var lines = File.ReadAllLines($"default_{_id}.txt");
            lines[1].Should().NotContain("321");
        }

        [TearDown]
        public void RemoveFile()
        {
            File.Delete($"default_{_id}.txt");
        }
    }
}
