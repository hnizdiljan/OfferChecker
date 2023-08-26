namespace ProductPriceNotificationApp
{
    public static class Services
    {
        // Property to store the service provider
        private static IServiceProvider _serviceProvider;

        // Method to add a service to the service provider
        public static void AddService<T>(T service) where T : class
        {
            // Create a new service collection
            var services = new ServiceCollection();

            // Add the service to the collection
            services.AddSingleton(service);

            // Create a new service provider using the services collection
            _serviceProvider = services.BuildServiceProvider();
        }

        // Method to get a service from the service provider
        public static T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
    }
}