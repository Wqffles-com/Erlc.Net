using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

/// <summary>
/// Information about a mod call.
/// </summary>
public class ModeratorCallLog
{
    /// <summary>
    /// The player who called the moderator.
    /// </summary>
    public required PlayerName Caller { get; set; }
    /// <summary>
    /// The moderator who answered the call.
    /// </summary>
    /// <remarks>
    /// Null if the call hasn't been answered yet.
    /// </remarks>
    public PlayerName? Moderator { get; set; }
    /// <summary>
    /// Timestamp when the call was made.
    /// </summary>
    /// <remarks>
    /// Measured in seconds since Unix Epoch.
    /// </remarks>
    public long Timestamp { get; set; }
    /// <summary>
    /// <see cref="Timestamp"/> converted to a <see cref="DateTimeOffset"/> for ease of use.
    /// </summary>
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
}