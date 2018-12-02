using BigFileAccessApi.Services;
using FluentAssertions;
using NUnit.Framework;

namespace BigFileAccessApi.Tests
{
    [TestFixture]
    public class IndexerShould
    {
        private IndexerService _indexer;

        [SetUp]
        public void IndexDefaultFile()
        {
            _indexer = new IndexerService(3);
            _indexer.IndexFile(@"default.txt");
        }

        [Test]
        public void AppendLineToEnd()
        {
            _indexer.AppendLine(10);
            _indexer.GetLine(3).Length.Should().Be(10);
        }

        [Test]
        public void AddLineInMiddle()
        {
            _indexer.AddLine(1, 10);
            _indexer.GetLine(1).Length.Should().Be(10);
        }

        [Test]
        public void GetLine()
        {
            _indexer.GetLine(0).Length.Should().Be(5);
            _indexer.GetLine(1).Length.Should().Be(3);
            _indexer.GetLine(2).Length.Should().Be(3);
        }
    }
}
