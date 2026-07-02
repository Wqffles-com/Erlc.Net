namespace Erlc.Net.Core.Models;

public class EmergencyCall : IEquatable<EmergencyCall>
{
    public string Team { get; set; } = string.Empty;
    public long Caller { get; set; }
    public List<string> Players { get; set; } = [];
    public List<float> Position { get; set; } = [];
    public long StartedAt { get; set; }
    public int CallNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PositionDescriptor { get; set; } = string.Empty;

    public bool Equals(EmergencyCall? other)
    {
        if (other is null) return false;
        return Team == other.Team
            && Caller == other.Caller
            && Players.SequenceEqual(other.Players)
            && Position.SequenceEqual(other.Position)
            && StartedAt == other.StartedAt
            && CallNumber == other.CallNumber
            && Description == other.Description
            && PositionDescriptor == other.PositionDescriptor;
    }

    public override bool Equals(object? obj) => Equals(obj as EmergencyCall);

    public override int GetHashCode()
    {
        var hc = new HashCode();
        hc.Add(Team);
        hc.Add(Caller);
        foreach (var p in Players) hc.Add(p);
        foreach (var p in Position) hc.Add(p);
        hc.Add(StartedAt);
        hc.Add(CallNumber);
        hc.Add(Description);
        hc.Add(PositionDescriptor);
        return hc.ToHashCode();
    }
}
