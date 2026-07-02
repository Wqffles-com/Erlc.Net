namespace Erlc.Net.Core.Models;

public class Location : IEquatable<Location>
{
    public float LocationX { get; set; }
    public float LocationZ { get; set; }
    public string? PostalCode { get; set; }
    public string? StreetName { get; set; }
    public string? BuildingNumber { get; set; }

    public bool Equals(Location? other)
    {
        if (other is null) return false;
        return LocationX.Equals(other.LocationX)
            && LocationZ.Equals(other.LocationZ)
            && PostalCode == other.PostalCode
            && StreetName == other.StreetName
            && BuildingNumber == other.BuildingNumber;
    }

    public override bool Equals(object? obj) => Equals(obj as Location);

    public override int GetHashCode()
    {
        return HashCode.Combine(LocationX, LocationZ, PostalCode, StreetName, BuildingNumber);
    }
}
