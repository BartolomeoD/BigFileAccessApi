using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BigFileAccessApi
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder<Startup>(args);
    }
}
