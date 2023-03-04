namespace PackZero.Configuration.Tests.Cases;

public class AppConfigurationExtensionsTests
{
    public AppConfigurationExtensionsTests()
    {
    }

    [Fact]
    public void IHostBuilder_Case_ActionIConfigurationBuilder()
    {
        JsonConfigurationSource taggedConfigurationSource = null;
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration((hostBuilderContext, configurationBuilder) =>
        {
            taggedConfigurationSource = (JsonConfigurationSource)configurationBuilder.Sources.First(f => f is JsonConfigurationSource cnf && cnf.Path != null && cnf.Path.Contains("appsettings.json"));
            configurationBuilder.Sources.Add(taggedConfigurationSource);
        });

        var wab = host.Build();

        using (var scope = wab.Services.CreateScope())
        {
            ConfigurationRoot? configuration = (ConfigurationRoot?)scope.ServiceProvider.GetService<IConfiguration>();
            configuration.ShouldNotBeNull();

            var matchWithTagged = configuration.Providers.Where(f => f is JsonConfigurationProvider json && json.Source.Path == taggedConfigurationSource?.Path).ToList();
            matchWithTagged.Count.ShouldBeEquivalentTo(2);
        }
    }

    [Fact]
    public void IHostBuilder_Case_EnvironmentName()
    {
        var randomEnvironmentName = Guid.NewGuid().ToString("N");
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration((hostBuilderContext, configurationBuilder) =>
        {
            hostBuilderContext.HostingEnvironment.EnvironmentName = randomEnvironmentName;
        });

        var wab = host.Build();

        using (var scope = wab.Services.CreateScope())
        {
            IHostEnvironment? hostEnvironment = scope.ServiceProvider.GetService<IHostEnvironment>();
            hostEnvironment.ShouldNotBeNull();
            hostEnvironment.EnvironmentName.ShouldNotBeNullOrWhiteSpace();
            hostEnvironment.EnvironmentName.ShouldBeEquivalentTo(randomEnvironmentName);
        }
    }

    [Fact]
    public void ServiceCollection_Case_NullServiceReference()
    {
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration().ConfigureServices(services =>
        {
            services.AddAppZeroConfiguration(typeof(NoAppsettingsScopeFoundConfig));
            services.AddSingleton<ServiceCollection_Case_NullServiceReference>();
        });

        var wab = host.Build();

        using (var scope = wab.Services.CreateScope())
        {
            ServiceCollection_Case_NullServiceReference? canHaveNullDIServiceReference = scope.ServiceProvider.GetService<ServiceCollection_Case_NullServiceReference>();
            canHaveNullDIServiceReference.ShouldNotBeNull();
            canHaveNullDIServiceReference.NoAppsettingsScopeFoundConfig.ShouldBeNull();
        }
    }

    [Fact]
    public void ServiceCollection_Case_ServiceScope()
    {
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration().ConfigureServices(services =>
        {
            services.AddAppZeroConfiguration(typeof(GeneralConfig));
            services.AddTransient<ServiceCollection_Case_ServiceScope>();
        });

        var wab = host.Build();

        Guid scope0GeneralConfigGuidId = Guid.Empty;
        Guid scope0GeneralServiceCollection_Case_ServiceScopeGuidId = Guid.Empty;
        using (var scope0 = wab.Services.CreateScope())
        {
            GeneralConfig? generalConfig = scope0.ServiceProvider.GetService<GeneralConfig>();
            generalConfig.ShouldNotBeNull();
            scope0GeneralConfigGuidId = generalConfig.UniqueId;

            ServiceCollection_Case_ServiceScope? canHaveNullDIServiceReference = scope0.ServiceProvider.GetService<ServiceCollection_Case_ServiceScope>();
            canHaveNullDIServiceReference.ShouldNotBeNull();
            scope0GeneralServiceCollection_Case_ServiceScopeGuidId = canHaveNullDIServiceReference.UniqueId;
            canHaveNullDIServiceReference.GeneralConfig.ShouldNotBeNull();
            canHaveNullDIServiceReference.GeneralConfig.UniqueId.ShouldNotBe(Guid.Empty);
            canHaveNullDIServiceReference.GeneralConfig.UniqueId.ShouldBeEquivalentTo(scope0GeneralConfigGuidId);
        }

        Guid scope1GeneralConfigGuidId = Guid.Empty;
        Guid scope1GeneralServiceCollection_Case_ServiceScopeGuidId = Guid.Empty;
        using (var scope1 = wab.Services.CreateScope())
        {
            GeneralConfig? generalConfig = scope1.ServiceProvider.GetService<GeneralConfig>();
            generalConfig.ShouldNotBeNull();
            scope1GeneralConfigGuidId = generalConfig.UniqueId;

            ServiceCollection_Case_ServiceScope? canHaveNullDIServiceReference = scope1.ServiceProvider.GetService<ServiceCollection_Case_ServiceScope>();
            canHaveNullDIServiceReference.ShouldNotBeNull();
            scope1GeneralServiceCollection_Case_ServiceScopeGuidId = canHaveNullDIServiceReference.UniqueId;
            canHaveNullDIServiceReference.GeneralConfig.ShouldNotBeNull();
            canHaveNullDIServiceReference.GeneralConfig.UniqueId.ShouldNotBe(Guid.Empty);
            canHaveNullDIServiceReference.GeneralConfig.UniqueId.ShouldBeEquivalentTo(scope1GeneralConfigGuidId);
        }

        scope0GeneralConfigGuidId.ShouldBe(scope1GeneralConfigGuidId);
        scope0GeneralServiceCollection_Case_ServiceScopeGuidId.ShouldNotBe(scope1GeneralServiceCollection_Case_ServiceScopeGuidId);
    }

    [Fact]
    public void ServiceCollection_Case_UndefinedServiceReference()
    {
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration();

        var wab = host.Build();

        using (var scope = wab.Services.CreateScope())
        {
            Assert.Throws<InvalidOperationException>(() => scope.ServiceProvider.GetRequiredService<GeneralConfig>());
        }
    }

    [Fact]
    public void ServiceCollection_Case_ValidServiceReference()
    {
        var host = Host.CreateDefaultBuilder().UseAppZeroConfiguration().ConfigureServices(services =>
         {
             services.AddAppZeroConfiguration(typeof(GeneralConfig));
         });

        var wab = host.Build();

        using (var scope = wab.Services.CreateScope())
        {
            GeneralConfig? generalConfig = scope.ServiceProvider.GetService<GeneralConfig>();
            generalConfig.ShouldNotBeNull();
            generalConfig.DiscordAppToken.ShouldNotBeNull();
            generalConfig.DiscordAppToken.ShouldBe("DevelperKey");
            generalConfig.DeveloperServerId.ShouldNotBe<ulong>(0);
            generalConfig.DeveloperIds.Count.ShouldBeGreaterThan(0);
        }
    }
}