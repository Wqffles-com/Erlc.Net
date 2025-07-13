using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class ModeratorCallLog
{
    public required string Caller { get; set; }
    public string CallerName => Caller.Split(':')[0];
    public string CallerId => Caller.Split(':')[1];
    public string? Moderator { get; set; }
    public string? ModeratorName => Moderator?.Split(':')[0];
    public string? ModeratorId => Moderator?.Split(':')[1];
    public long Timestamp { get; set; }
    [JsonIgnore] public DateTimeOffset DateTime => DateTimeOffset.FromUnixTimeSeconds(Timestamp);
    
}