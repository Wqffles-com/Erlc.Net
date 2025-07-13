using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class CommandLog
{
    public required string Player { get; set; }
    public string PlayerName => Player.Split(':')[0];
    public string PlayerId => Player.Split(':')[1];
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    public required string Command { get; set; }
}