using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Erlc.Net.Entities;
using Erlc.Net.Exceptions;

namespace Erlc.Net;

/// <summary>
/// A client used to interact with PRC's API.
/// </summary>
/// <param name="accessToken">The access token to use. Can be obtained at PRC's website.</param>
/// <param name="version">PRC API version to use.</param>
public class ErlcClient(string accessToken, byte version = 1)
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri($"https://api.policeroleplay.community/v{version}"),
        DefaultRequestHeaders =
        {
            { "server-key", accessToken }
        }
    };

    private async Task<TResult> Request<TResult>(HttpRequestMessage message)
    {
        var response = await _httpClient.SendAsync(message);
        InvalidTokenException.ThrowIfInvalid(response);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<TResult>() ?? throw new JsonException($"Unable to parse response: {response}");
    }

    private async Task Request(HttpRequestMessage message)
    {
        var response = await _httpClient.SendAsync(message);
        InvalidTokenException.ThrowIfInvalid(response);
        response.EnsureSuccessStatusCode();
    }

    public Task RunCommand(string command) => Request(new HttpRequestMessage(HttpMethod.Post, "server/command")
    {
        Content = JsonContent.Create(new { command })
    });
    
    public async Task<Server> GetServer()
    {
        var server = await Request<Server>(new HttpRequestMessage(HttpMethod.Get, "server"));
        server.Client = this;
        
        return server;
    }

    public Task<ErlcPlayer[]> GetPlayers() => Request<ErlcPlayer[]>(new HttpRequestMessage(HttpMethod.Get, "server/players"));

    public Task<JoinLog[]> GetJoinLogs() => Request<JoinLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/joinlogs"));

    public Task<ulong[]> GetPlayersInQueue() => Request<ulong[]>(new HttpRequestMessage(HttpMethod.Get, "server/queue"));
    
    public Task<KillLog[]> GetKillLogs() => Request<KillLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/killlogs"));

    public Task<CommandLog[]> GetCommandLogs() => Request<CommandLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/commandlogs"));

    public Task<ModeratorCallLog[]> GetModCallLogs() => Request<ModeratorCallLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/modcalls"));

    public Task<Dictionary<string, string>> GetBans() => Request<Dictionary<string, string>>(new HttpRequestMessage(HttpMethod.Get, "server/bans"));

    public Task<SpawnedVehicle[]> GetSpawnedVehicles() => Request<SpawnedVehicle[]>(new HttpRequestMessage(HttpMethod.Get, "server/vehicles"));
}