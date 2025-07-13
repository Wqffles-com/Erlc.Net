namespace Erlc.Net.Entities;

public class ErlcPlayer
{
    public required PlayerName Player { get; set; }
    public required string Permission { get; set; }
    public string? Callsign { get; set; }
    public required string Team { get; set; }
}