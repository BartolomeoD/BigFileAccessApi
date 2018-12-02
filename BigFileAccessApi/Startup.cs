using System;
using System.IO;
using BigFileAccessApi.Abstractions;
using BigFileAccessApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BigFileAccessApi
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return new FileStream(config["App:BigFilePath"], FileMode.Open);
            });

            services.AddSingleton<IBigFileReader, BigFileReader>();

            services.AddSingleton<IBigFileWriter, BigFileWriter>();

            services.AddSingleton<IIndexerService>(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                var indexerService = new IndexerService(Int32.Parse(config["App:IndexDefaultBufferSize"]));
                return indexerService;
            });
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStarted.Register(() =>
            {
                var config = app.ApplicationServices.GetService<IConfiguration>();
                var indexerService = app.ApplicationServices.GetService<IIndexerService>();
                indexerService.IndexFile(config["App:BigFilePath"]);
            });
            appLifetime.ApplicationStopping.Register(() =>
            {
                var bigFileWriter = app.ApplicationServices.GetService<IBigFileWriter>();
                bigFileWriter.Close();
                var bigFileReader = app.ApplicationServices.GetService<IBigFileReader>();
                bigFileReader.Close();
            });
            app.UseMvc();
        }
    }
}
