namespace Erlc.Net.Extensions.Hosting;

public class ErlcOptions
{
    public int DefaultPollingIntervalSeconds { get; set; } = 15;
    public bool EnablePollingByDefault { get; set; } = true;
}
