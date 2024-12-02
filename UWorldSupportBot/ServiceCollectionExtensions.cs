using DialogFlowAPI;

namespace UWorldSupportBot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDialogFlowService(this IServiceCollection services)
        {
            services.AddScoped<DialogFlowAPIService>();
            return services;
        }
    }
}
