using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductPriceNotificationApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProductPriceNotificationApp
{
    public class App : Application
    {
        // Method to initialize the app
        public override async Task InitializeAsync()
        {
            // Initialize Xamarin Essentials
            await Xamarin.Essentials.InitAsync();

            // Add a singleton service for the AppViewModel
            Services.AddService<AppViewModel>();

            // Register the AppViewModel as the root component
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build();
            host.Services
                .GetRequiredService<NavigationManager>()
                .NavigateTo(nameof(AppViewModel));
        }
    }

    // Class to configure the app
    public class Startup
    {
        // Method to configure the app services
        public void ConfigureServices(IServiceCollection services)
        {
            // Add the NavigationManager service
            services.AddScoped<NavigationManager>(provider =>
            {
                var navManager = new NavigationManager();
                navManager.Initialize(provider.GetRequiredService<IHostApplicationLifecycle>());
                return navManager;
            });

            // Add the AppViewModel as a singleton service
            services.AddSingleton<AppViewModel>();
        }

        // Method to configure the app
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure logging
            loggerFactory.AddProvider(new ConsoleLoggerProvider());

            // Use HTTP endpoints to handle requests
            app.UseRouting();

            // Use the AppViewModel to handle requests
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage(nameof(AppViewModel));
            });
        }
    }
}