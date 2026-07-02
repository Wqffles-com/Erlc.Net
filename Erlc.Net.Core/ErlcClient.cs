using System.Runtime.Serialization;
using System.Text;
using Erlc.Net.Core.Models;
using Newtonsoft.Json;

#pragma warning disable SYSLIB0050 // StreamingContext is required by Newtonsoft.Json

namespace Erlc.Net.Core;

public class ErlcClientOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string ServerId { get; set; } = string.Empty;
    public int PollingIntervalSeconds { get; set; } = 15;
    public bool EnablePolling { get; set; } = true;
}

public class ErlcClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _serializerSettings;
    private CancellationTokenSource _serverInfoEventsCancellationTokenSource = new();
    private readonly int _pollingIntervalSeconds;
    private ServerInfo? _cachedServerInfo;
    private bool _disposed;
    private Task? _pollingTask;

    public string ServerId { get; }

    public event EventHandler<ServerInfoChangedEventArgs>? ServerDetailsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<Player>>>? PlayersChanged;
    public event EventHandler<ServerInfoChangedEventArgs<Staff>>? StaffChanged;
    public event EventHandler<ServerInfoChangedEventArgs<Dictionary<string, string>>>? AdminsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<Dictionary<string, string>>>? ModsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<Dictionary<string, string>>>? HelpersChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<JoinLog>>>? JoinLogsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<int>>>? QueueChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<KillLog>>>? KillLogsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<CommandLog>>>? CommandLogsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<ModCall>>>? ModCallsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<EmergencyCall>>>? EmergencyCallsChanged;
    public event EventHandler<ServerInfoChangedEventArgs<List<Vehicle>>>? VehiclesChanged;

    public ErlcClient(string apiKey, HttpClient? httpClient = null)
        : this(new ErlcClientOptions { ApiKey = apiKey, EnablePolling = false }, httpClient)
    {
    }

    public ErlcClient(ErlcClientOptions options, HttpClient? httpClient = null)
    {
        ServerId = options.ServerId;
        _pollingIntervalSeconds = options.PollingIntervalSeconds;

        _httpClient = httpClient ?? new HttpClient();
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://api.erlc.gg/");
        }

        _httpClient.DefaultRequestHeaders.Add("Server-Key", options.ApiKey);

        _serializerSettings = new JsonSerializerSettings
        {
            Context = new StreamingContext(StreamingContextStates.Other, this)
        };
    }

    public void StartPolling()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_pollingTask != null)
            return;

        _pollingTask = Task.Run(() => RunServerInfoEventsAsync(_serverInfoEventsCancellationTokenSource.Token));
    }

    public void StopPolling()
    {
        var task = _pollingTask;
        if (task == null)
            return;

        _serverInfoEventsCancellationTokenSource.Cancel();

        try
        {
            task.Wait(TimeSpan.FromSeconds(5));
        }
        catch
        {
        }

        _serverInfoEventsCancellationTokenSource.Dispose();
        _serverInfoEventsCancellationTokenSource = new CancellationTokenSource();
        _pollingTask = null;
    }

    public async Task<ServerInfo> GetServerInfoAsync(
        bool? players = null,
        bool? staff = null,
        bool? joinLogs = null,
        bool? queue = null,
        bool? killLogs = null,
        bool? commandLogs = null,
        bool? modCalls = null,
        bool? emergencyCalls = null,
        bool? vehicles = null,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new List<string>();
        if (players.HasValue) queryParams.Add($"Players={players.Value.ToString().ToLower()}");
        if (staff.HasValue) queryParams.Add($"Staff={staff.Value.ToString().ToLower()}");
        if (joinLogs.HasValue) queryParams.Add($"JoinLogs={joinLogs.Value.ToString().ToLower()}");
        if (queue.HasValue) queryParams.Add($"Queue={queue.Value.ToString().ToLower()}");
        if (killLogs.HasValue) queryParams.Add($"KillLogs={killLogs.Value.ToString().ToLower()}");
        if (commandLogs.HasValue) queryParams.Add($"CommandLogs={commandLogs.Value.ToString().ToLower()}");
        if (modCalls.HasValue) queryParams.Add($"ModCalls={modCalls.Value.ToString().ToLower()}");
        if (emergencyCalls.HasValue) queryParams.Add($"EmergencyCalls={emergencyCalls.Value.ToString().ToLower()}");
        if (vehicles.HasValue) queryParams.Add($"Vehicles={vehicles.Value.ToString().ToLower()}");

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

        var response = await _httpClient.GetAsync($"v2/server{queryString}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response);
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var serverInfo = JsonConvert.DeserializeObject<ServerInfo>(json, _serializerSettings);

        return serverInfo ?? throw new InvalidOperationException("Failed to deserialize server info.");
    }

    public async Task RunCommandAsync(string command, CancellationToken cancellationToken = default)
    {
        var payload = new { Command = command };
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("v2/server/command", content, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await HandleErrorResponseAsync(response);
        }
    }

    private async Task HandleErrorResponseAsync(HttpResponseMessage response)
    {
        var statusCode = (int)response.StatusCode;
        if (statusCode is 400 or 422 or 500)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErlcErrorResponse>(errorJson);
                throw new ErlcApiException(statusCode, errorResponse);
            }
            catch (JsonException)
            {
            }
        }

        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        StopPolling();
        _serverInfoEventsCancellationTokenSource.Dispose();
        _httpClient.Dispose();
    }

    private async Task RunServerInfoEventsAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_pollingIntervalSeconds));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await PollServerInfoAsync(cancellationToken);
                await timer.WaitForNextTickAsync(cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch
            {
                await timer.WaitForNextTickAsync(cancellationToken);
            }
        }
    }

    private async Task PollServerInfoAsync(CancellationToken cancellationToken)
    {
        var serverInfo = await GetServerInfoAsync(
            players: true,
            staff: true,
            joinLogs: true,
            queue: true,
            killLogs: true,
            commandLogs: true,
            modCalls: true,
            emergencyCalls: true,
            vehicles: true,
            cancellationToken: cancellationToken);

        var cachedServerInfo = _cachedServerInfo;
        if (cachedServerInfo != null)
        {
            RaiseChangedEvents(cachedServerInfo, serverInfo);
        }

        _cachedServerInfo = serverInfo;
    }

    private void RaiseChangedEvents(ServerInfo previous, ServerInfo current)
    {
        if (IsServerDetailsChanged(previous, current))
        {
            ServerDetailsChanged?.Invoke(this, new ServerInfoChangedEventArgs(previous, current));
        }

        RaiseIfChanged(PlayersChanged, previous.Players, current.Players, previous, current);
        RaiseIfChanged(StaffChanged, previous.Staff, current.Staff, previous, current);
        RaiseIfChanged(AdminsChanged, previous.Staff.Admins, current.Staff.Admins, previous, current);
        RaiseIfChanged(ModsChanged, previous.Staff.Mods, current.Staff.Mods, previous, current);
        RaiseIfChanged(HelpersChanged, previous.Staff.Helpers, current.Staff.Helpers, previous, current);
        RaiseIfChanged(JoinLogsChanged, previous.JoinLogs, current.JoinLogs, previous, current);
        RaiseIfChanged(QueueChanged, previous.Queue, current.Queue, previous, current);
        RaiseIfChanged(KillLogsChanged, previous.KillLogs, current.KillLogs, previous, current);
        RaiseIfChanged(CommandLogsChanged, previous.CommandLogs, current.CommandLogs, previous, current);
        RaiseIfChanged(ModCallsChanged, previous.ModCalls, current.ModCalls, previous, current);
        RaiseIfChanged(EmergencyCallsChanged, previous.EmergencyCalls, current.EmergencyCalls, previous, current);
        RaiseIfChanged(VehiclesChanged, previous.Vehicles, current.Vehicles, previous, current);
    }

    private static bool IsServerDetailsChanged(ServerInfo previous, ServerInfo current)
    {
        return previous.Name != current.Name ||
               previous.OwnerId != current.OwnerId ||
               !AreEqual(previous.CoOwnerIds, current.CoOwnerIds) ||
               previous.CurrentPlayers != current.CurrentPlayers ||
               previous.MaxPlayers != current.MaxPlayers ||
               previous.JoinKey != current.JoinKey ||
               previous.AccVerifiedReq != current.AccVerifiedReq ||
               previous.TeamBalance != current.TeamBalance;
    }

    private void RaiseIfChanged<T>(
        EventHandler<ServerInfoChangedEventArgs<T>>? handler,
        T previous,
        T current,
        ServerInfo previousServerInfo,
        ServerInfo currentServerInfo)
    {
        if (!AreEqual(previous, current))
        {
            handler?.Invoke(this, new ServerInfoChangedEventArgs<T>(previous, current, previousServerInfo, currentServerInfo));
        }
    }

    private static bool AreEqual<T>(T previous, T current)
    {
        if (previous is null && current is null) return true;
        if (previous is null || current is null) return false;

        if (previous is IEquatable<T> equatable)
            return equatable.Equals(current);

        if (previous is System.Collections.IList prevList && current is System.Collections.IList currList)
        {
            if (prevList.Count != currList.Count) return false;
            for (var i = 0; i < prevList.Count; i++)
            {
                if (!Equals(prevList[i], currList[i]))
                    return false;
            }
            return true;
        }

        if (previous is System.Collections.IDictionary prevDict && current is System.Collections.IDictionary currDict)
        {
            if (prevDict.Count != currDict.Count) return false;
            foreach (var key in prevDict.Keys)
            {
                if (!currDict.Contains(key) || !Equals(prevDict[key], currDict[key]))
                    return false;
            }
            return true;
        }

        return EqualityComparer<T>.Default.Equals(previous, current);
    }
}

public class ServerInfoChangedEventArgs : EventArgs
{
    public ServerInfoChangedEventArgs(ServerInfo previousServerInfo, ServerInfo currentServerInfo)
    {
        PreviousServerInfo = previousServerInfo;
        CurrentServerInfo = currentServerInfo;
    }

    public ServerInfo PreviousServerInfo { get; }
    public ServerInfo CurrentServerInfo { get; }
}

public class ServerInfoChangedEventArgs<T> : ServerInfoChangedEventArgs
{
    public ServerInfoChangedEventArgs(T previous, T current, ServerInfo previousServerInfo, ServerInfo currentServerInfo)
        : base(previousServerInfo, currentServerInfo)
    {
        Previous = previous;
        Current = current;
    }

    public T Previous { get; }
    public T Current { get; }
}
