using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

/// <summary>
/// Information about a player joining the server.
/// </summary>
public class JoinLog
{
    /// <summary>
    /// Whether the player joined or left the server.
    /// </summary>
    public required bool Join { get; set; }
    /// <summary>
    /// Timestamp when the player joined or left the server.
    /// </summary>
    /// <remarks>
    /// Measured in seconds since Unix Epoch.
    /// </remarks>
    public long Timestamp { get; set; }
    /// <summary>
    /// <see cref="Timestamp"/> converted to a <see cref="DateTimeOffset"/> for ease of use.
    /// </summary>
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    /// <summary>
    /// The player who joined or left the server.
    /// </summary>
    public required PlayerName Player { get; set; }
}