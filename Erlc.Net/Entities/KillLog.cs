using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

/// <summary>
/// Information about a player being killed.
/// </summary>
public class KillLog
{
    /// <summary>
    /// The player who was killed.
    /// </summary>
    public required PlayerName Killed { get; set; }
    /// <summary>
    /// Timestamp when the player was killed.
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
    /// The player who killed the player.
    /// </summary>
    public required PlayerName Killer { get; set; }
}