using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class CommandLog
{
    public required PlayerName Player { get; set; }
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    public required string Command { get; set; }
}