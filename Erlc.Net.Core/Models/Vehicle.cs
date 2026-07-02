namespace Erlc.Net.Core.Models;

public class Vehicle : IEquatable<Vehicle>
{
    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public string Texture { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;

    public bool Equals(Vehicle? other)
    {
        if (other is null) return false;
        return Name == other.Name
            && Owner == other.Owner
            && Plate == other.Plate
            && Texture == other.Texture
            && ColorHex == other.ColorHex
            && ColorName == other.ColorName;
    }

    public override bool Equals(object? obj) => Equals(obj as Vehicle);

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Owner, Plate, Texture, ColorHex, ColorName);
    }
}
