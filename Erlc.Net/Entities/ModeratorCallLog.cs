using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class ModeratorCallLog
{
    public required PlayerName Caller { get; set; }
    public PlayerName? Moderator { get; set; }
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    
}