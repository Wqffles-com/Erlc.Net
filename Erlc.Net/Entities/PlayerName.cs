using System.Text.Json.Serialization;

namespace Erlc.Net.Entities;

public class PlayerName
{
    public required string Player { get; set; }
    [JsonIgnore] public string Name => Player.Split(':')[0];
    [JsonIgnore] public string Id => Player.Split(':')[1];
    
    public static implicit operator string(PlayerName playerName) => playerName.Player;
    public static implicit operator PlayerName?(string? playerName) => playerName != null ? new() { Player = playerName } : null;
}