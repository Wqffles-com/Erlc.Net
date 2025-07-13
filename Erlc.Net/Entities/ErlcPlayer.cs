namespace Erlc.Net.Entities;

public class ErlcPlayer
{
    public required string Player { get; set; }
    public string PlayerName => Player.Split(':')[0];
    public string PlayerId => Player.Split(':')[1];
    public required string Permission { get; set; }
    public string? Callsign { get; set; }
    public required string Team { get; set; }
}