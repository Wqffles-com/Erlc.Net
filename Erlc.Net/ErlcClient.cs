using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Erlc.Net.Entities;
using Erlc.Net.Exceptions;

namespace Erlc.Net;

/// <summary>
/// Provides methods for interacting with an ERLC server via API.
/// </summary>
public class ErlcClient(string accessToken, byte version = 1)
{
    /// <summary>
    /// Represents an instance of the HttpClient used for sending HTTP requests
    /// to the API endpoint. It is pre-configured with the base address and default
    /// headers required for authentication and API versioning.
    /// </summary>
    /// <remarks>
    /// This HttpClient is specifically initialized with the access token and the
    /// API version provided during the creation of the <see cref="ErlcClient"/> instance.
    /// It is used internally to facilitate all API interactions.
    /// </remarks>
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri($"https://api.policeroleplay.community/v{version}"),
        DefaultRequestHeaders =
        {
            { "server-key", accessToken }
        }
    };

    /// Sends an HTTP request and processes the response as the specified result type.
    /// <typeparam name="TResult">The type of the expected response content.</typeparam>
    /// <param name="message">The HTTP request message to be sent.</param>
    /// <returns>The content of the response deserialized into the specified result type.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to the specified type.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    private async Task<TResult> Request<TResult>(HttpRequestMessage message)
    {
        var response = await _httpClient.SendAsync(message);
        InvalidTokenException.ThrowIfInvalid(response);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<TResult>() ?? throw new JsonException($"Unable to parse response: {response}");
    }

    /// <summary>
    /// Sends an HTTP request using the provided HttpRequestMessage.
    /// </summary>
    /// <param name="message">The HttpRequestMessage to be sent.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the provided token is invalid.</exception>
    /// <exception cref="HttpRequestException">Thrown when the request fails or the response status code indicates a failure.</exception>
    private async Task Request(HttpRequestMessage message)
    {
        var response = await _httpClient.SendAsync(message);
        InvalidTokenException.ThrowIfInvalid(response);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Sends a command to the ERLC server for execution.
    /// </summary>
    /// <param name="command">The command to run on the server.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RunCommand(string command) => Request(new HttpRequestMessage(HttpMethod.Post, "server/command")
    {
        Content = JsonContent.Create(new { command })
    });

    /// Retrieves information about the ERLC server.
    /// <returns>An instance of the <see cref="Server"/> class containing details about the server and providing methods for server interaction.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is unauthorized due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized into the <see cref="Server"/> type.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public async Task<Server> GetServer()
    {
        var server = await Request<Server>(new HttpRequestMessage(HttpMethod.Get, "server"));
        server.Client = this;
        
        return server;
    }

    /// Retrieves the list of players currently online on the server.
    /// <returns>An array of <see cref="ErlcPlayer"/> objects representing the active players on the server.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to an array of <see cref="ErlcPlayer"/>.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<ErlcPlayer[]> GetPlayers() => Request<ErlcPlayer[]>(new HttpRequestMessage(HttpMethod.Get, "server/players"));

    /// Retrieves the server's join logs, detailing information about players joining the server.
    /// <returns>An array of join log entries, representing details about players joining the server.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized into the expected type.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<JoinLog[]> GetJoinLogs() => Request<JoinLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/joinlogs"));

    /// Retrieves the list of player identifiers currently in the server queue.
    /// <returns>An array of unsigned long integers, each representing a player identifier in the queue.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to the expected type.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<ulong[]> GetPlayersInQueue() => Request<ulong[]>(new HttpRequestMessage(HttpMethod.Get, "server/queue"));

    /// <summary>
    /// Retrieves a list of kill logs from the server, providing details about player kills.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of <see cref="KillLog"/> objects.</returns>
    public Task<KillLog[]> GetKillLogs() => Request<KillLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/killlogs"));

    /// Retrieves the logs of executed commands from the server.
    /// <returns>An array of <see cref="CommandLog"/> objects representing the logs of executed commands.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to the <see cref="CommandLog"/> type.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<CommandLog[]> GetCommandLogs() => Request<CommandLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/commandlogs"));

    /// Retrieves all moderator call logs from the server.
    /// Returns an array of ModeratorCallLog objects, each containing information about a mod call.
    /// <returns>
    /// An array of ModeratorCallLog objects representing the moderator call logs.
    /// </returns>
    public Task<ModeratorCallLog[]> GetModCallLogs() => Request<ModeratorCallLog[]>(new HttpRequestMessage(HttpMethod.Get, "server/modcalls"));

    /// Retrieves the list of bans from the ERLC server.
    /// <returns>A dictionary where the keys are user identifiers and the values are the respective user names.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to a dictionary.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<Dictionary<string, string>> GetBans() => Request<Dictionary<string, string>>(new HttpRequestMessage(HttpMethod.Get, "server/bans"));

    /// Retrieves a list of all spawned vehicles in the server.
    /// <returns>An array of <see cref="SpawnedVehicle"/> representing the currently spawned vehicles.</returns>
    /// <exception cref="InvalidTokenException">Thrown if the request is forbidden due to an invalid access token.</exception>
    /// <exception cref="JsonException">Thrown if the response content cannot be deserialized to an array of SpawnedVehicle.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails or encounters success status issues.</exception>
    public Task<SpawnedVehicle[]> GetSpawnedVehicles() => Request<SpawnedVehicle[]>(new HttpRequestMessage(HttpMethod.Get, "server/vehicles"));
}