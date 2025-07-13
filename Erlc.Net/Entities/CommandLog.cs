using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

/// <summary>
/// Information about the execution of a command.
/// </summary>
public class CommandLog
{
    /// <summary>
    /// Moderator who executed the command.
    /// </summary>
    public required PlayerName Player { get; set; }
    /// <summary>
    /// Timestamp when the command was executed.
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
    /// The actual command that was run.
    /// </summary>
    /// <example>
    /// :h Hello guys!
    /// </example>
    public required string Command { get; set; }
}