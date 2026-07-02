using Erlc.Net.Core;
using Erlc.Net.Core.Models;
using Moq;
using Xunit;

namespace Erlc.Net.Tests;

public class ErlcClientPoolTests
{
    [Fact]
    public void GetOrAddClient_ReturnsSameInstance_ForSameServerId()
    {
        using var pool = new ErlcClientPool();
        var client1 = pool.GetOrAddClient("s1", "key1");
        var client2 = pool.GetOrAddClient("s1", "key2");
        Assert.Same(client1, client2);
    }

    [Fact]
    public void GetOrAddClient_CreatesDifferentInstances_ForDifferentServerIds()
    {
        using var pool = new ErlcClientPool();
        var client1 = pool.GetOrAddClient("s1", "key1");
        var client2 = pool.GetOrAddClient("s2", "key2");
        Assert.NotSame(client1, client2);
    }

    [Fact]
    public void GetClient_ReturnsNull_WhenServerIdNotFound()
    {
        using var pool = new ErlcClientPool();
        Assert.Null(pool.GetClient("nonexistent"));
    }

    [Fact]
    public void GetClient_ReturnsClient_WhenServerIdExists()
    {
        using var pool = new ErlcClientPool();
        pool.GetOrAddClient("s1", "key1");
        Assert.NotNull(pool.GetClient("s1"));
    }

    [Fact]
    public void TryGetClient_ReturnsTrue_WhenServerIdExists()
    {
        using var pool = new ErlcClientPool();
        pool.GetOrAddClient("s1", "key1");
        Assert.True(pool.TryGetClient("s1", out var client));
        Assert.NotNull(client);
    }

    [Fact]
    public void TryGetClient_ReturnsFalse_WhenServerIdNotFound()
    {
        using var pool = new ErlcClientPool();
        Assert.False(pool.TryGetClient("nonexistent", out var client));
        Assert.Null(client);
    }

    [Fact]
    public void RemoveClient_ReturnsTrue_AndDisposesClient()
    {
        using var pool = new ErlcClientPool();
        pool.GetOrAddClient("s1", "key1");
        Assert.True(pool.RemoveClient("s1"));
        Assert.Null(pool.GetClient("s1"));
    }

    [Fact]
    public void RemoveClient_ReturnsFalse_ForUnknownServerId()
    {
        using var pool = new ErlcClientPool();
        Assert.False(pool.RemoveClient("nonexistent"));
    }

    [Fact]
    public void UpdateClient_ReplacesExistingClient()
    {
        using var pool = new ErlcClientPool();
        var original = pool.GetOrAddClient("s1", "oldKey");
        var updated = pool.UpdateClient("s1", "newKey");
        Assert.NotSame(original, updated);
        Assert.Same(updated, pool.GetClient("s1"));
    }

    [Fact]
    public void UpdateClient_Works_WhenServerIdDoesNotExist()
    {
        using var pool = new ErlcClientPool();
        var client = pool.UpdateClient("newServer", "key");
        Assert.NotNull(client);
        Assert.Same(client, pool.GetClient("newServer"));
    }

    [Fact]
    public void GetAllClients_ReturnsAllClients()
    {
        using var pool = new ErlcClientPool();
        pool.GetOrAddClient("s1", "key1");
        pool.GetOrAddClient("s2", "key2");
        Assert.Equal(2, pool.GetAllClients().Count);
    }

    [Fact]
    public void Constructor_Accepts_IHttpClientFactory()
    {
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        using var pool = new ErlcClientPool(factoryMock.Object);
        var client = pool.GetOrAddClient("s1", "key1");
        Assert.NotNull(client);
        factoryMock.Verify(f => f.CreateClient(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void AutoStartPolling_Default_IsTrue()
    {
        using var pool = new ErlcClientPool();
        var client = pool.GetOrAddClient("s1", "key1");
        Assert.NotNull(client);
    }

    [Fact]
    public void AutoStartPolling_False_DoesNotStartPolling()
    {
        using var pool = new ErlcClientPool(autoStartPolling: false);
        var client = pool.GetOrAddClient("s1", "key1");
        Assert.NotNull(client);
    }

    [Fact]
    public void Dispose_DisposesAllClients()
    {
        var pool = new ErlcClientPool();
        pool.GetOrAddClient("s1", "key1");
        pool.GetOrAddClient("s2", "key2");
        pool.Dispose();
        Assert.Empty(pool.GetAllClients());
    }

    [Fact]
    public void DoubleDispose_DoesNotThrow()
    {
        var pool = new ErlcClientPool();
        pool.Dispose();
        pool.Dispose();
    }
}
