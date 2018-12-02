using System.IO;
using System.Threading.Tasks;
using BigFileAccessApi.Contracts;
using BigFileAccessApi.Services;
using FluentAssertions;
using NUnit.Framework;

namespace BigFileAccessApi.Tests
{
    public class BigFileReaderShould
    {
        private BigFileReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new BigFileReader(new FileStream("default.txt", FileMode.Open));
        }

        [Test]
        public async Task ReadLine()
        {
            var line = new Line
            {
                StartOffset = 6,
                Length = 3
            };
            var str = await _reader.ReadLineAsync(line);
            str.Should().Be("321");
        }
    }
}