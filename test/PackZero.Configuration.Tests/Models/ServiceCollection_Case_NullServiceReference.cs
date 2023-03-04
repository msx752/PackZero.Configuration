namespace PackZero.Configuration.Tests.Models;

public class ServiceCollection_Case_NullServiceReference
{
    public ServiceCollection_Case_NullServiceReference(NoAppsettingsScopeFoundConfig noAppsettingsScopeFoundConfig)
    {
        NoAppsettingsScopeFoundConfig = noAppsettingsScopeFoundConfig;
    }

    public NoAppsettingsScopeFoundConfig NoAppsettingsScopeFoundConfig { get; }
}