namespace PackZero.Configuration;

public static class AppConfigurationExtensions
{
    public static IHostBuilder UseAppZeroConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseAppZeroConfiguration(null);
    }

    public static IHostBuilder UseAppZeroConfiguration(this IHostBuilder hostBuilder, Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        return hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
        {
            hostBuilderContext.HostingEnvironment.EnvironmentName =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            configurationBuilder.Sources.Clear();
            configurationBuilder
                .AddCommandLine(Environment.GetCommandLineArgs())
                .AddEnvironmentVariables()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", false, true);

            configureDelegate?.Invoke(hostBuilderContext, configurationBuilder);
        });
    }

    public static IServiceCollection AddAppZeroConfiguration(this IServiceCollection services, params Type[] appSettingSectionTypes)
    {
        foreach (var appSetting in appSettingSectionTypes.Distinct())
        {
            services
                .AddSingleton(appSetting, (provider) => provider.GetRequiredService<IConfiguration>().GetSection(appSetting.Name).Get(appSetting));
        }
        return services;
    }
}