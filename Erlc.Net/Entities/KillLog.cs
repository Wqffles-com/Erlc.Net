using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class KillLog
{
    public required string Killed { get; set; }
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    public required PlayerName Killer { get; set; }
}