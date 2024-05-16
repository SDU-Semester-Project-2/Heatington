using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SourceDataManagerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //only here for testing
            // SourceDataManagerModel.SDM.LogTimeSeriesData();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
