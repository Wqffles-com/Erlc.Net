namespace Erlc.Net.Core.Models;

public class Staff : IEquatable<Staff>
{
    public Dictionary<string, string> Admins { get; set; } = new();
    public Dictionary<string, string> Mods { get; set; } = new();
    public Dictionary<string, string> Helpers { get; set; } = new();

    public bool Equals(Staff? other)
    {
        if (other is null) return false;
        return DictionaryEqual(Admins, other.Admins)
            && DictionaryEqual(Mods, other.Mods)
            && DictionaryEqual(Helpers, other.Helpers);
    }

    public override bool Equals(object? obj) => Equals(obj as Staff);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            DictionaryHashCode(Admins),
            DictionaryHashCode(Mods),
            DictionaryHashCode(Helpers));
    }

    private static bool DictionaryEqual(Dictionary<string, string> a, Dictionary<string, string> b)
    {
        if (a.Count != b.Count) return false;
        foreach (var kvp in a)
        {
            if (!b.TryGetValue(kvp.Key, out var val) || val != kvp.Value)
                return false;
        }
        return true;
    }

    private static int DictionaryHashCode(Dictionary<string, string> dict)
    {
        var hc = new HashCode();
        foreach (var kvp in dict)
        {
            hc.Add(kvp.Key);
            hc.Add(kvp.Value);
        }
        return hc.ToHashCode();
    }
}
