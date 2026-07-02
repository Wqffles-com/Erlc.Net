using Erlc.Net.Core;
using Xunit;

namespace Erlc.Net.Tests;

public class PollingLifecycleTests
{
    [Fact]
    public void StartPolling_ThenStopPolling_DoesNotThrow()
    {
        using var client = new ErlcClient("testApiKey");
        client.StartPolling();
        client.StopPolling();
    }

    [Fact]
    public void StartPolling_IsIdempotent()
    {
        using var client = new ErlcClient("testApiKey");
        client.StartPolling();
        client.StartPolling();
        client.StartPolling();
        client.StopPolling();
    }

    [Fact]
    public void StopPolling_WhenNotStarted_DoesNotThrow()
    {
        using var client = new ErlcClient("testApiKey");
        client.StopPolling();
    }

    [Fact]
    public void StopPolling_ThenStartPolling_Works()
    {
        using var client = new ErlcClient("testApiKey");
        client.StartPolling();
        client.StopPolling();
        client.StartPolling();
        client.StopPolling();
    }

    [Fact]
    public void StartPolling_AfterDispose_Throws()
    {
        var client = new ErlcClient("testApiKey");
        client.Dispose();
        Assert.Throws<ObjectDisposedException>(() => client.StartPolling());
    }

    [Fact]
    public void Dispose_StopsPolling()
    {
        var client = new ErlcClient("testApiKey");
        client.StartPolling();
        client.Dispose();
        client.StopPolling();
    }
}
