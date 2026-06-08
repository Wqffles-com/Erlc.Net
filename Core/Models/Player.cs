using Newtonsoft.Json;

namespace Core.Models;

public class Player : BaseEntity
{
    public string Team { get; set; } = string.Empty;
    
    [JsonProperty("Player")]
    public string RawPlayer { get; set; } = string.Empty;
    
    public string Callsign { get; set; } = string.Empty;
    public Location? Location { get; set; }
    public string Permission { get; set; } = string.Empty;
    public int WantedStars { get; set; }

    [JsonIgnore]
    public string Name => RawPlayer.Split(':')[0];

    [JsonIgnore]
    public long Id => long.TryParse(RawPlayer.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task Ban(string reason = "")
    {
        return GetClient().RunCommandAsync($":ban {Name} {reason}");
    }

    public Task Kick(string reason = "")
    {
        return GetClient().RunCommandAsync($":kick {Name} {reason}");
    }
    
    public Task Kill()
    {
        return GetClient().RunCommandAsync($":kill {Name}");
    }
    
    public Task Respawn()
    {
        return GetClient().RunCommandAsync($":respawn {Name}");
    }
}
