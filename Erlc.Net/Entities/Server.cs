using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class Server : IContainsClient
{
    [JsonIgnore] public required ErlcClient Client { get; set; }
    public required string Name { get; set; }
    public required ulong OwnerId { get; set; }
    public ulong[] CoOwnerIds { get; set; } = [];
    public required byte CurrentPlayers { get; set; }
    public required byte MaxPlayers { get; set; }
    public required string JoinKey { get; set; }
    public required string AccVerifiedReq { get; set; }
    public required bool TeamBalance { get; set; }
    
    public Task<ErlcResponse> ExecuteCommand(string command) => Client.RunCommand(command);
    public Task<ErlcResponse<ErlcPlayer[]>> GetPlayers => Client.GetPlayers();
    public Task<ErlcResponse<JoinLog[]>> GetJoinLogs => Client.GetJoinLogs();
    public Task<ErlcResponse<ulong[]>> GetPlayersInQueue => Client.GetPlayersInQueue();
    public Task<ErlcResponse<KillLog[]>> GetKillLogs => Client.GetKillLogs();
    public Task<ErlcResponse<CommandLog[]>> GetCommandLogs => Client.GetCommandLogs();
    public Task<ErlcResponse<ModeratorCallLog[]>> GetModCallLogs => Client.GetModCallLogs();
    public Task<ErlcResponse<Dictionary<string, string>>> GetBans => Client.GetBans();
    public Task<ErlcResponse<SpawnedVehicle[]>> GetSpawnedVehicles => Client.GetSpawnedVehicles();
}