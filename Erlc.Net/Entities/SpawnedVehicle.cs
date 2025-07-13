namespace Erlc.Net.Entities;

/// <summary>
/// Information about a spawned vehicle.
/// </summary>
public class SpawnedVehicle
{
    /// <summary>
    /// The vehicle's texture.
    /// </summary>
    /// <example>
    /// Default
    /// Staff Team
    /// </example>
    public string? Texture { get; set; }
    /// <summary>
    /// The vehicle's name.
    /// </summary>
    /// <example>
    /// Falcon Interceptor Utility 2019
    /// </example>
    public required string Name { get; set; }
    /// <summary>
    /// The vehicle's owner.
    /// </summary>
    public required string Owner { get; set; }
}