using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BigFileAccessApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = new List<Line>();
            var defaultBufferSize = 100;
            int bufferSize;
            var buffer = new byte[defaultBufferSize];
            var offset = 0;
            long currentStringOffset = 0;
            long newLineOffset = 0;
            var currentStringNumber = 0;
            var fileName = @"D:\test.txt";
            var fileLength = new FileInfo(fileName).Length;

            var stopwatch = new Stopwatch();
            long a = 0;
            long b = 0;
            stopwatch.Start();
            try
            {
                using (var file = MemoryMappedFile.CreateFromFile(fileName))
                {
                    using (var accessor = file.CreateViewAccessor(0, fileLength))
                    {
                        Console.WriteLine("Index start");
                        do
                        {
                            if (offset + defaultBufferSize > fileLength)
                                bufferSize = (int)(fileLength - offset);
                            else
                                bufferSize = defaultBufferSize;
                            accessor.ReadArray(offset, buffer, 0, bufferSize);
                            var str = System.Text.Encoding.UTF8.GetString(buffer);
                            //Console.Write(str);
                            offset += bufferSize;
                            var prevNewLineOffset = 0;
                            var found = false;
                            for (var i = 0; i < bufferSize; i++)
                            {
                                if (buffer[i] == (byte) '\n' )
                                {
                                    found = true;
                                    newLineOffset += i - prevNewLineOffset;
                                    lines.Add(new Line
                                    {
                                        Number = currentStringNumber,
                                        Length = newLineOffset - currentStringOffset,
                                        StartOffset = currentStringOffset
                                    });
                                    currentStringNumber++;
                                    currentStringOffset = newLineOffset + 1;
                                    prevNewLineOffset = i;
                                }
                            }

                            if (!found)
                                newLineOffset += bufferSize;
                        } while (offset < fileLength);

                        Console.WriteLine("Index end");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            using (var file = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                foreach (var line in lines)
                {
                    break;
                    var bytes_data = new byte[line.Length];
                    file.Seek(line.StartOffset, SeekOrigin.Begin);
                    file.Read(bytes_data, 0, bytes_data.Length);
                    var str = System.Text.Encoding.UTF8.GetString(bytes_data);
                    //Console.WriteLine(str);
                }
            }
            Console.WriteLine(lines.Count);
            Console.ReadKey();
            //CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder<Startup>(args);

        private class Line
        {
            public long Number { get; set; }
            public long StartOffset { get; set; }
            public long Length { get; set; }
        }
    }
}
