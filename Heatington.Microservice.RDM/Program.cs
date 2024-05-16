namespace Heatington.Microservice.RDM;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();

        _ = ResultDataManagerModel.LoadResultDataManager();

        var service = new ResultDataManagerService();

        _ = service.GetDataFromOpt();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            });
}
