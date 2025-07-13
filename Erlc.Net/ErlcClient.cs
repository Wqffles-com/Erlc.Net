using System.Net;
using System.Net.Http.Json;
using Erlc.Net.Entities;

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

    /// <inheritdoc cref="Server.RunCommand(string)"/>
    public async Task<ErlcResponse> RunCommand(string command)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "server/command")
        {
            Content = JsonContent.Create(new { command })
        };
        
        var response = await _httpClient.SendAsync(request);

        var errorMessage = response.StatusCode switch
        {
            HttpStatusCode.BadRequest => "Invalid command",
            HttpStatusCode.Forbidden => "Invalid access token",
            HttpStatusCode.UnprocessableEntity => "Server contains no players",
            HttpStatusCode.InternalServerError => "Problem communicating with Roblox",
            _ => null
        };
        
        return new ErlcResponse
        {
            Success = response.IsSuccessStatusCode,
            StatusCode = response.StatusCode,
            ErrorMessage = errorMessage
        };
    }
    
    /// <summary>
    /// Gets the server associated with this API Key.
    /// </summary>
    /// <returns>A filled <see cref="Server"/> object.</returns>
    public async Task<ErlcResponse<Server>> GetServer()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<Server>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<Server>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }

        var server = await response.Content.ReadFromJsonAsync<Server>();
        if (server is null)
        {
            var responseEntity = new ErlcResponse<Server>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse server response: {response}"
            };
            
            return responseEntity;
        }
        
        server.Client = this;

        return new ErlcResponse<Server>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = server
        };
    }

    /// <inheritdoc cref="Server.GetPlayers"/>
    public async Task<ErlcResponse<ErlcPlayer[]>> GetPlayers()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/players");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<ErlcPlayer[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }

        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<ErlcPlayer[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }

        var players = await response.Content.ReadFromJsonAsync<ErlcPlayer[]>();
        if (players is null)
        {
            var responseEntity = new ErlcResponse<ErlcPlayer[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse players response: {response}"
            };
            
            return responseEntity;
        }
        
        return new ErlcResponse<ErlcPlayer[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = players
        };
    }

    /// <inheritdoc cref="Server.GetJoinLogs()"/>
    public async Task<ErlcResponse<JoinLog[]>> GetJoinLogs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/joinlogs");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<JoinLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };
            
            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<JoinLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };
            
            return responseEntity;
        }

        var logs = await response.Content.ReadFromJsonAsync<JoinLog[]>();
        if (logs is null)
        {
            var responseEntity = new ErlcResponse<JoinLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse join logs response: {response}"
            };
            
            return responseEntity;
        }
        
        return new ErlcResponse<JoinLog[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = logs
        };
    }

    /// <inheritdoc cref="Server.GetPlayersInQueue()"/>
    public async Task<ErlcResponse<ulong[]>> GetPlayersInQueue()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/queue");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<ulong[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };
            
            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<ulong[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };
        }

        var queue = await response.Content.ReadFromJsonAsync<ulong[]>();
        if (queue is null)
        {
            var responseEntity = new ErlcResponse<ulong[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse queue response: {response}"
            };
        }
        
        return new ErlcResponse<ulong[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = queue
        };
    }
    
    /// <inheritdoc cref="Server.GetKillLogs()"/>
    public async Task<ErlcResponse<KillLog[]>> GetKillLogs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/killlogs");
        var response = await _httpClient.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<KillLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };
            
            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<KillLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }

        var killLogs = await response.Content.ReadFromJsonAsync<KillLog[]>();
        if (killLogs is null)
        {
            var responseEntity = new ErlcResponse<KillLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse kill logs response: {response}"
            };
        }
        
        return new ErlcResponse<KillLog[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = killLogs
        };
    }

    /// <inheritdoc cref="Server.GetCommandLogs()"/>
    public async Task<ErlcResponse<CommandLog[]>> GetCommandLogs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/commandlogs");
        var response = await _httpClient.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<CommandLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<CommandLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }
        
        var commandLogs = await response.Content.ReadFromJsonAsync<CommandLog[]>();
        if (commandLogs is null)
        {
            var responseEntity = new ErlcResponse<CommandLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse command logs response: {response}"
            };

            return responseEntity;
        }

        return new ErlcResponse<CommandLog[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = commandLogs
        };
    }

    /// <inheritdoc cref="Server.GetModCallLogs()"/>
    public async Task<ErlcResponse<ModeratorCallLog[]>> GetModCallLogs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/modcalls");
        var response = await _httpClient.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<ModeratorCallLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<ModeratorCallLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }
        
        var moderatorCallLogs = await response.Content.ReadFromJsonAsync<ModeratorCallLog[]>();
        if (moderatorCallLogs is null)
        {
            var responseEntity = new ErlcResponse<ModeratorCallLog[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse mod call logs response: {response}"
            };

            return responseEntity;
        }

        return new ErlcResponse<ModeratorCallLog[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = moderatorCallLogs
        };
    }

    /// <inheritdoc cref="Server.GetBans()"/>
    public async Task<ErlcResponse<Dictionary<string, string>>> GetBans()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/bans");
        var response = await _httpClient.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<Dictionary<string, string>>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<Dictionary<string, string>>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }
        
        var commandLogs = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        if (commandLogs is null)
        {
            var responseEntity = new ErlcResponse<Dictionary<string, string>>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse command logs response: {response}"
            };

            return responseEntity;
        }

        return new ErlcResponse<Dictionary<string, string>>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = commandLogs
        };
    }

    /// <inheritdoc cref="Server.GetSpawnedVehicles()"/>
    public async Task<ErlcResponse<SpawnedVehicle[]>> GetSpawnedVehicles()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "server/vehicles");
        var response = await _httpClient.SendAsync(request);
        
        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            var responseEntity = new ErlcResponse<SpawnedVehicle[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = "Invalid access token"
            };

            return responseEntity;
        }
        
        if (!response.IsSuccessStatusCode)
        {
            var responseEntity = new ErlcResponse<SpawnedVehicle[]>
            {
                Success = false,
                StatusCode = response.StatusCode
            };

            return responseEntity;
        }
        
        var commandLogs = await response.Content.ReadFromJsonAsync<SpawnedVehicle[]>();
        if (commandLogs is null)
        {
            var responseEntity = new ErlcResponse<SpawnedVehicle[]>
            {
                Success = false,
                StatusCode = response.StatusCode,
                ErrorMessage = $"Unable to parse command logs response: {response}"
            };

            return responseEntity;
        }

        return new ErlcResponse<SpawnedVehicle[]>
        {
            Success = true,
            StatusCode = response.StatusCode,
            Result = commandLogs
        };
    }
}