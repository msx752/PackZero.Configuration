namespace PackZero.Configuration.Tests.Models;

public class GeneralConfig
{
    public Guid UniqueId { get; }

    public GeneralConfig()
    {
        DeveloperIds = new List<ulong>();
        UniqueId = Guid.NewGuid();
    }

    public string DiscordAppToken { get; set; }
    public List<ulong> DeveloperIds { get; set; }
    public ulong DeveloperServerId { get; set; }
}