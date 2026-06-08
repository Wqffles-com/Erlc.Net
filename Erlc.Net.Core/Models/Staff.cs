namespace Erlc.Net.Core.Models;

public class Staff
{
    public Dictionary<string, string> Admins { get; set; } = new();
    public Dictionary<string, string> Mods { get; set; } = new();
    public Dictionary<string, string> Helpers { get; set; } = new();
}
