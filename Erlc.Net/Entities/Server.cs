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
    /// The current number of players on the server.
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
}