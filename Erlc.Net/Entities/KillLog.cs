using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class KillLog
{
    public required string Killed { get; set; }
    public string KilledName => Killed.Split(':')[0];
    public string KilledId => Killed.Split(':')[1];
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    public required string Killer { get; set; }
    public string KillerName => Killer.Split(':')[0];
    public string KillerId => Killer.Split(':')[1];
}