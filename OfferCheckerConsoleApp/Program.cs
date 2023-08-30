using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = CreateHostBuilder(args).Build();

        var serviceProvider = host.Services.CreateScope().ServiceProvider;
        var controller = serviceProvider.GetRequiredService<ProductController>();

        string configPath = "config.json";

        await controller.Run(configPath);
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddHttpClient();
                 services.Configure<TelegramSettings>(hostContext.Configuration.GetSection("TelegramSettings"));
                 services.AddSingleton<ICrawler, DefaultCrawler>();
                 services.AddHttpClient<ICrawler, DefaultCrawler>(client =>
                 {
                     client.DefaultRequestHeaders.Add("User-Agent", "ZboziCrawler");
                     // Další nastavení, pokud je potřeba
                 });
                 services.AddSingleton<ITelegramNotifier, TelegramNotifier>();
                 services.AddSingleton<IPriceComparisonService, PriceComparisonService>();
                 services.AddTransient<ProductController>();
             })
             .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
             {
                 configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
                 configurationBuilder.AddJsonFile("appsettings.json", optional: false);
             });
}