namespace PackZero.Configuration.Tests.Models;

public class ServiceCollection_Case_ServiceScope
{
    public Guid UniqueId { get; }

    public ServiceCollection_Case_ServiceScope(GeneralConfig generalConfig)
    {
        GeneralConfig = generalConfig;
        UniqueId = Guid.NewGuid();
    }

    public GeneralConfig GeneralConfig { get; }
}