namespace Erlc.Net.Core.Models;

public class ServerInfo : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public long OwnerId { get; set; }
    public List<long> CoOwnerIds { get; set; } = [];
    public int CurrentPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public string JoinKey { get; set; } = string.Empty;
    public string AccVerifiedReq { get; set; } = string.Empty;
    public bool TeamBalance { get; set; }
    public List<Player> Players { get; set; } = [];
    public Staff Staff { get; set; } = new();
    public List<JoinLog> JoinLogs { get; set; } = [];
    public List<int> Queue { get; set; } = [];
    public List<KillLog> KillLogs { get; set; } = [];
    public List<CommandLog> CommandLogs { get; set; } = [];
    public List<ModCall> ModCalls { get; set; } = [];
    public List<EmergencyCall> EmergencyCalls { get; set; } = [];
    public List<Vehicle> Vehicles { get; set; } = [];

    public Task RunCommand(string command)
    {
        return GetClient().RunCommandAsync(command);
    }
}
