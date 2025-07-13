namespace Erlc.Net.Entities;

/// <summary>
/// Information about an online server member.
/// </summary>
public class ErlcPlayer
{
    /// <summary>
    /// Player's name
    /// </summary>
    public required PlayerName Player { get; set; }
    /// <summary>
    /// What permission level the player has.
    /// </summary>
    /// <example>
    /// Normal / Server Administrator / Server Owner / Server Moderator
    /// </example>
    public required string Permission { get; set; }
    /// <summary>
    /// The player's callsign.
    /// </summary>
    /// <remarks>
    /// Only available if the player is on a non-civilian team.
    /// </remarks>
    public string? Callsign { get; set; }
    /// <summary>
    /// The team the player is playing on.
    /// </summary>
    public required string Team { get; set; }
}