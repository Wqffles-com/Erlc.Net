namespace Core.Models;

public class EmergencyCall
{
    public string Team { get; set; } = string.Empty;
    public long Caller { get; set; }
    public List<string> Players { get; set; } = [];
    public List<float> Position { get; set; } = [];
    public long StartedAt { get; set; }
    public int CallNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PositionDescriptor { get; set; } = string.Empty;
}
