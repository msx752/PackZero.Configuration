namespace PackZero.Configuration;

public static class AppConfigurationExtensions
{
    public static IHostBuilder UseAppZeroConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseAppZeroConfiguration(null);
    }

    public static IHostBuilder UseAppZeroConfiguration(this IHostBuilder hostBuilder, Action<HostBuilderContext, IConfigurationBuilder> configureDelegate, string appSettingSubPath = null)
    {
        return hostBuilder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
        {
            var removedDefaultEnvironment = configurationBuilder.Sources.FirstOrDefault(f => f is JsonConfigurationSource cnf
                && cnf.Path != null
                && cnf.Path.Contains("appsettings.Production.json")
                );

            if (removedDefaultEnvironment != null)
                configurationBuilder.Sources.Remove(removedDefaultEnvironment);

            var pathAppSetting = (JsonConfigurationSource)configurationBuilder.Sources.First(f => f is JsonConfigurationSource cnf && cnf.Path != null && cnf.Path.Contains("appsettings.json"));
            pathAppSetting.Path = Path.Combine(appSettingSubPath?.Trim(' ') ?? "", "appsettings.json");

            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            hostBuilderContext.HostingEnvironment.EnvironmentName = string.IsNullOrWhiteSpace(env) ? "Development" : env;

            configurationBuilder.AddCommandLine(Environment.GetCommandLineArgs()).AddEnvironmentVariables();

            configureDelegate?.Invoke(hostBuilderContext, configurationBuilder);

            var pathEnvFile = Path.Combine(appSettingSubPath?.Trim(' ') ?? "", $"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json");
            configurationBuilder.AddJsonFile(pathEnvFile, optional: true, reloadOnChange: true);
        });
    }

    public static IServiceCollection AddAppZeroConfiguration(this IServiceCollection services, params Type[] appSettingSectionTypes)
    {
        foreach (var appSettingType in appSettingSectionTypes.Distinct())
        {
            services.AddSingleton(appSettingType, (provider) =>
                    provider.GetRequiredService<IConfiguration>().GetSection(appSettingType.Name).Get(appSettingType));
        }

        return services;
    }
}