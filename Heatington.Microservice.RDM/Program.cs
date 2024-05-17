namespace Heatington.Microservice.RDM;

public class Program
{
    public static void Main(string[] args)
    {
        ResultDataManagerModel.LoadResultDataManager();

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
}
