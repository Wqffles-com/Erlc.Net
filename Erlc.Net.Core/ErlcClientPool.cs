using System.Collections.Concurrent;

namespace Erlc.Net.Core;

public class ErlcClientPool : IDisposable
{
    private readonly ConcurrentDictionary<string, ErlcClient> _clients = new();
    private readonly IHttpClientFactory? _httpClientFactory;
    private readonly bool _autoStartPolling;
    private bool _disposed;

    public ErlcClientPool(IHttpClientFactory? httpClientFactory = null, bool autoStartPolling = true)
    {
        _httpClientFactory = httpClientFactory;
        _autoStartPolling = autoStartPolling;
    }

    public ErlcClient GetOrAddClient(string serverId, string apiKey, int pollingIntervalSeconds = 15)
    {
        return _clients.GetOrAdd(serverId, _ =>
        {
            var httpClient = _httpClientFactory?.CreateClient(nameof(ErlcClient));
            var options = new ErlcClientOptions
            {
                ServerId = serverId,
                ApiKey = apiKey,
                PollingIntervalSeconds = pollingIntervalSeconds,
                EnablePolling = false
            };

            var client = new ErlcClient(options, httpClient);
            if (_autoStartPolling)
                client.StartPolling();
            return client;
        });
    }

    public ErlcClient? GetClient(string serverId)
    {
        _clients.TryGetValue(serverId, out var client);
        return client;
    }

    public bool TryGetClient(string serverId, out ErlcClient? client)
    {
        return _clients.TryGetValue(serverId, out client);
    }

    public ErlcClient UpdateClient(string serverId, string newApiKey, int? pollingIntervalSeconds = null)
    {
        if (_clients.TryRemove(serverId, out var existing))
        {
            existing.StopPolling();
            existing.Dispose();
        }

        var httpClient = _httpClientFactory?.CreateClient(nameof(ErlcClient));
        var options = new ErlcClientOptions
        {
            ServerId = serverId,
            ApiKey = newApiKey,
            PollingIntervalSeconds = pollingIntervalSeconds ?? 15,
            EnablePolling = false
        };

        var client = new ErlcClient(options, httpClient);
        if (_autoStartPolling)
            client.StartPolling();

        _clients[serverId] = client;
        return client;
    }

    public bool RemoveClient(string serverId)
    {
        if (_clients.TryRemove(serverId, out var client))
        {
            client.StopPolling();
            client.Dispose();
            return true;
        }

        return false;
    }

    public IReadOnlyCollection<ErlcClient> GetAllClients()
    {
        return _clients.Values.ToList().AsReadOnly();
    }

    public void StartAll()
    {
        foreach (var client in _clients.Values)
        {
            client.StartPolling();
        }
    }

    public void StopAll()
    {
        foreach (var client in _clients.Values)
        {
            client.StopPolling();
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        foreach (var client in _clients.Values)
        {
            client.Dispose();
        }

        _clients.Clear();
    }
}
