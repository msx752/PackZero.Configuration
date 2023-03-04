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
            var removedDefaultEnvironment = configurationBuilder.Sources.FirstOrDefault(f => f is JsonConfigurationSource cnf
                && cnf.Path != null
                && cnf.Path.Contains("appsettings.Production.json")
                );

            if (removedDefaultEnvironment != null)
                configurationBuilder.Sources.Remove(removedDefaultEnvironment);

            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            hostBuilderContext.HostingEnvironment.EnvironmentName = string.IsNullOrWhiteSpace(env) ? "Development" : env;

            configurationBuilder.AddCommandLine(Environment.GetCommandLineArgs()).AddEnvironmentVariables();

            configureDelegate?.Invoke(hostBuilderContext, configurationBuilder);

            configurationBuilder.AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        });
    }

    public static IServiceCollection AddAppZeroConfiguration(this IServiceCollection services, params Type[] appSettingSectionTypes)
    {
        foreach (var appSettingType in appSettingSectionTypes.Distinct())
            services.AddSingleton(appSettingType, (provider) =>
                provider.GetRequiredService<IConfiguration>().GetSection(appSettingType.Name).Get(appSettingType));

        return services;
    }
}