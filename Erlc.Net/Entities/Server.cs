using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

/// <summary>
/// Information about and methods for an ERLC server.
/// </summary>
public class Server : IContainsClient
{
    [JsonIgnore] public required ErlcClient Client { get; set; }
    public required string Name { get; set; }
    public required ulong OwnerId { get; set; }
    public ulong[] CoOwnerIds { get; set; } = [];
    /// <summary>
    /// Currently active players.
    /// </summary>
    public required byte CurrentPlayers { get; set; }
    /// <summary>
    /// Maximum players allowed on the server.
    /// </summary>
    /// <remarks>
    /// One spot is reserved for the server owner.
    /// </remarks>
    public required byte MaxPlayers { get; set; }
    /// <summary>
    /// The key players can use to join the server.
    /// </summary>
    /// <remarks>
    /// Should typically not be given to players, since it can start the server.
    /// </remarks>
    public required string JoinKey { get; set; }
    /// <summary>
    /// Account verification required to join the server.
    /// </summary>
    public required string AccVerifiedReq { get; set; }
    /// <summary>
    /// Whether teams have to be balanced.
    /// If true, players will be spread across teams.
    /// </summary>
    public required bool TeamBalance { get; set; }
    
    /// <summary>
    /// Executes a command on the server.
    /// </summary>
    /// <param name="command">The command to run.</param>
    /// <returns>Filled out <see cref="ErlcResponse"/></returns>
    public Task<ErlcResponse> RunCommand(string command) => Client.RunCommand(command);
    /// <summary>
    /// Gets every currently active player on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every active player.</returns>
    public Task<ErlcResponse<ErlcPlayer[]>> GetPlayers => Client.GetPlayers();
    /// <summary>
    /// Gets every join log on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every join log.</returns>
    public Task<ErlcResponse<JoinLog[]>> GetJoinLogs => Client.GetJoinLogs();
    /// <summary>
    /// Gets every player in the queue.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every player in the queue.</returns>
    public Task<ErlcResponse<ulong[]>> GetPlayersInQueue => Client.GetPlayersInQueue();
    /// <summary>
    /// Gets every kill log on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every kill log.</returns>
    public Task<ErlcResponse<KillLog[]>> GetKillLogs => Client.GetKillLogs();
    /// <summary>
    /// Gets every command log on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every command log.</returns>
    public Task<ErlcResponse<CommandLog[]>> GetCommandLogs => Client.GetCommandLogs();
    /// <summary>
    /// Gets every moderator call log on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every moderator call log.</returns>
    public Task<ErlcResponse<ModeratorCallLog[]>> GetModCallLogs => Client.GetModCallLogs();
    /// <summary>
    /// Gets every ban on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every ban.</returns>
    public Task<ErlcResponse<Dictionary<string, string>>> GetBans => Client.GetBans();
    /// <summary>
    /// Gets every spawned vehicle on the server.
    /// </summary>
    /// <returns>Filled out <see cref="ErlcResponse{T}"/> with every spawned vehicle.</returns>
    public Task<ErlcResponse<SpawnedVehicle[]>> GetSpawnedVehicles => Client.GetSpawnedVehicles();
}