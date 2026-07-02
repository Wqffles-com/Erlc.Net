using Newtonsoft.Json;

namespace Erlc.Net.Core.Models;

public class JoinLog : BaseEntity, IEquatable<JoinLog>
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

    public bool Equals(JoinLog? other)
    {
        if (other is null) return false;
        return Join == other.Join && Timestamp == other.Timestamp && Player == other.Player;
    }

    public override bool Equals(object? obj) => Equals(obj as JoinLog);

    public override int GetHashCode() => HashCode.Combine(Join, Timestamp, Player);
}

public class KillLog : BaseEntity, IEquatable<KillLog>
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

    public bool Equals(KillLog? other)
    {
        if (other is null) return false;
        return Killed == other.Killed && Timestamp == other.Timestamp && Killer == other.Killer;
    }

    public override bool Equals(object? obj) => Equals(obj as KillLog);

    public override int GetHashCode() => HashCode.Combine(Killed, Timestamp, Killer);
}

public class CommandLog : BaseEntity, IEquatable<CommandLog>
{
    public string Player { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public string Command { get; set; } = string.Empty;

    [JsonIgnore]
    public string Name => Player.Split(':')[0];
    [JsonIgnore]
    public long Id => long.TryParse(Player.Split(':').ElementAtOrDefault(1), out var id) ? id : 0;

    public Task Ban(string reason = "") => GetClient().RunCommandAsync($":ban {Name} {reason}");

    public bool Equals(CommandLog? other)
    {
        if (other is null) return false;
        return Player == other.Player && Timestamp == other.Timestamp && Command == other.Command;
    }

    public override bool Equals(object? obj) => Equals(obj as CommandLog);

    public override int GetHashCode() => HashCode.Combine(Player, Timestamp, Command);
}

public class ModCall : BaseEntity, IEquatable<ModCall>
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

    public bool Equals(ModCall? other)
    {
        if (other is null) return false;
        return Caller == other.Caller && Moderator == other.Moderator && Timestamp == other.Timestamp;
    }

    public override bool Equals(object? obj) => Equals(obj as ModCall);

    public override int GetHashCode() => HashCode.Combine(Caller, Moderator, Timestamp);
}
