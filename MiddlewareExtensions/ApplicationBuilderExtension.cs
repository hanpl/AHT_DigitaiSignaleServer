using DigitalSignageSevice.SubscribeTableDependencies;

namespace DigitalSignageSevice.MiddlewareExtensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseDigitalSignalrTableDependency(this IApplicationBuilder applicationBuilder, string connectionString)
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var service = serviceProvider.GetService<SubscribeDigitalSignageTableDependency>();
            service.SubscribeTableDependency(connectionString);
        }
    }
}
