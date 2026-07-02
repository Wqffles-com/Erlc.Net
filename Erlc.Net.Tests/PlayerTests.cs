using Erlc.Net.Core.Models;
using Xunit;

namespace Erlc.Net.Tests;

public class PlayerTests
{
    [Fact]
    public void Name_ParsesCorrectly()
    {
        var player = new Player { RawPlayer = "testUser:12345" };
        Assert.Equal("testUser", player.Name);
    }

    [Fact]
    public void Id_ParsesCorrectly()
    {
        var player = new Player { RawPlayer = "testUser:12345" };
        Assert.Equal(12345, player.Id);
    }

    [Fact]
    public void Id_ReturnsZero_WhenNoColon()
    {
        var player = new Player { RawPlayer = "testUser" };
        Assert.Equal(0, player.Id);
    }

    [Fact]
    public void Player_Equals_ComparesAllFields()
    {
        var loc1 = new Location { LocationX = 100, LocationZ = 200, PostalCode = "12345" };
        var loc2 = new Location { LocationX = 100, LocationZ = 200, PostalCode = "12345" };
        var p1 = new Player { Team = "Police", RawPlayer = "user:1", Callsign = "C1", Location = loc1, Permission = "全员", WantedStars = 3 };
        var p2 = new Player { Team = "Police", RawPlayer = "user:1", Callsign = "C1", Location = loc2, Permission = "全员", WantedStars = 3 };
        Assert.Equal(p1, p2);
    }

    [Fact]
    public void Player_Equals_ReturnsFalse_WhenDifferent()
    {
        var p1 = new Player { RawPlayer = "user:1", Team = "Police" };
        var p2 = new Player { RawPlayer = "user:2", Team = "Police" };
        Assert.NotEqual(p1, p2);
    }

    [Fact]
    public void Player_Equals_Null_ReturnsFalse()
    {
        var p1 = new Player { RawPlayer = "user:1" };
        Assert.False(p1.Equals(null));
    }

    [Fact]
    public void Player_GetHashCode_ConsistentWithEquals()
    {
        var p1 = new Player { Team = "Police", RawPlayer = "user:1", Callsign = "C1" };
        var p2 = new Player { Team = "Police", RawPlayer = "user:1", Callsign = "C1" };
        Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
    }

    [Fact]
    public void Kick_WithNameContainingSpaces_UsesFullName()
    {
        var player = new Player { RawPlayer = "test user:12345" };
        Assert.Equal("test user", player.Name);
    }

    [Fact]
    public void Team_DefaultsToEmpty()
    {
        var player = new Player();
        Assert.Equal(string.Empty, player.Team);
    }
}
