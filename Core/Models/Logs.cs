using Newtonsoft.Json;

namespace Core.Models;

public class JoinLog : BaseEntity
{
    public bool Join { get; set; }
    public long Timestamp { get; set; }
    public string Player { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string Name => Player.Split(':')[0];
    
    [JsonIgnore]
    public long Id => long.TryParse(Player.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task Ban(string reason = "") => GetClient().RunCommandAsync($":ban {Name} {reason}");
    public Task Kick(string reason = "") => GetClient().RunCommandAsync($":kick {Name} {reason}");
}

public class KillLog : BaseEntity
{
    public string Killed { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public string Killer { get; set; } = string.Empty;

    [JsonIgnore]
    public string KilledName => Killed.Split(':')[0];
    [JsonIgnore]
    public long KilledId => long.TryParse(Killed.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    [JsonIgnore]
    public string KillerName => Killer.Split(':')[0];
    [JsonIgnore]
    public long KillerId => long.TryParse(Killer.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task BanKiller(string reason = "") => GetClient().RunCommandAsync($":ban {KillerName} {reason}");
    public Task BanKilled(string reason = "") => GetClient().RunCommandAsync($":ban {KilledName} {reason}");
}

public class CommandLog : BaseEntity
{
    public string Player { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public string Command { get; set; } = string.Empty;

    [JsonIgnore]
    public string Name => Player.Split(':')[0];
    [JsonIgnore]
    public long Id => long.TryParse(Player.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task Ban(string reason = "") => GetClient().RunCommandAsync($":ban {Name} {reason}");
}

public class ModCall : BaseEntity
{
    public string Caller { get; set; } = string.Empty;
    public string Moderator { get; set; } = string.Empty;
    public long Timestamp { get; set; }

    [JsonIgnore]
    public string CallerName => Caller.Split(':')[0];
    [JsonIgnore]
    public long CallerId => long.TryParse(Caller.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    [JsonIgnore]
    public string ModeratorName => Moderator.Split(':')[0];
    [JsonIgnore]
    public long ModeratorId => long.TryParse(Moderator.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task BanCaller(string reason = "") => GetClient().RunCommandAsync($":ban {CallerName} {reason}");
}
