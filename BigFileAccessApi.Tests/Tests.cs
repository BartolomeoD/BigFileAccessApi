using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using NUnit.Framework;

namespace BigFileAccessApi.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test()
        {
            var lines = new List<Line>();
            var defaultBufferSize = 100;
            var bufferSize = 0;
            var buffer = new byte[defaultBufferSize];
            var offset = 0;
            var currentStringOffset = 0;
            var newLineOffset = 0;
            var currentStringlength = 0;
            var currentStringNumber = 0;
            var fileName = @"D:\test.log";
            var fileLength = new FileInfo(fileName).Length;
            try
            {
                using (var file = MemoryMappedFile.CreateFromFile(fileName))
                {
                    using (var accessor = file.CreateViewAccessor(0, fileLength))
                    {
                        do
                        {
                            if (offset + defaultBufferSize > accessor.Capacity)
                                bufferSize = (int) (accessor.Capacity - offset);
                            else
                                bufferSize = defaultBufferSize;
                            accessor.ReadArray(offset, buffer, 0, bufferSize);
                            var str = System.Text.Encoding.UTF8.GetString(buffer);
                            Console.Write(str);
                            offset += bufferSize;
//                        var newLinePosition = str.IndexOf("\r\n", StringComparison.Ordinal);
//                        if (newLinePosition > -1)
//                        {
//                            newLineOffset += newLinePosition;
//
//                            lines.Add(new Line
//                            {
//                                Number = currentStringNumber,
//                                Length = newLineOffset - currentStringOffset,
//                                StartOffset = currentStringOffset
//                            });
//                        }
                        } while (offset < fileLength);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [Test]
        public void Test1()
        {
            var stopwatch = new Stopwatch();
            long a = 0;
            long b = 0;
            stopwatch.Start();
            using (var file = File.Open(@"D:\test.log", FileMode.Open, FileAccess.Write))
            {
                stopwatch.Stop();
                a = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();
                var bytes = System.Text.Encoding.UTF8.GetBytes("my string");
                file.Seek(1000000, SeekOrigin.Begin);
                file.Write(bytes, 0, bytes.Length);
                Thread.Sleep(1);
                b = stopwatch.ElapsedMilliseconds;
            }
        }

        private class Line
        {
            public long Number { get; set; }
            public long StartOffset { get; set; }
            public long Length { get; set; }
        }
    }
}