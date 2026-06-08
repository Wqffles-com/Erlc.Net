using System.Runtime.Serialization;
using System.Text;
using Core.Models;
using Newtonsoft.Json;

namespace Core;

public class ErlcClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _serializerSettings;

    public ErlcClient(string apiKey, HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://api.erlc.gg/");
        }

        _httpClient.DefaultRequestHeaders.Add("Server-Key", apiKey);

        _serializerSettings = new JsonSerializerSettings
        {
            Context = new StreamingContext(StreamingContextStates.Other, this)
        };
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
        bool? vehicles = null)
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

        var response = await _httpClient.GetAsync($"v2/server{queryString}");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var serverInfo = JsonConvert.DeserializeObject<ServerInfo>(json, _serializerSettings);
        
        return serverInfo ?? throw new InvalidOperationException("Failed to deserialize server info.");
    }

    public async Task RunCommandAsync(string command)
    {
        var payload = new { Command = command };
        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("v2/server/command", content);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
