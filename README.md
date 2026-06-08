# Erlc.Net

Erlc.Net is a .NET client library for the Roblox Emergency Response: Liberty County (ER:LC) private server API. It provides a typed wrapper around the ER:LC HTTP endpoints, model classes for server data, convenience helpers for moderation commands, and optional `Microsoft.Extensions.DependencyInjection` integration.

## Packages

| Package | Description |
| --- | --- |
| `Erlc.Net.Core` | Core `ErlcClient`, API models, command helpers, and server change events. |
| `Erlc.Net.Extensions.Hosting` | Dependency injection and `IHttpClientFactory` integration for hosted .NET apps. |

Both packages currently target `net10.0`.

## Features

- Fetch server details from the ER:LC `v2/server` endpoint.
- Optionally include players, staff, join logs, queue, kill logs, command logs, moderator calls, emergency calls, and vehicles in one request.
- Run ER:LC server commands through the API.
- Subscribe to polling-based change events for server details and included collections.
- Use typed models such as `ServerInfo`, `Player`, `Staff`, `JoinLog`, `KillLog`, `CommandLog`, `ModCall`, `EmergencyCall`, and `Vehicle`.
- Register the client with `IServiceCollection` using configuration, an API key string, or an options delegate.

## Installation

Install the core package in applications that create `ErlcClient` directly:

```bash
dotnet add package Erlc.Net.Core
```

Install the hosting package when using dependency injection:

```bash
dotnet add package Erlc.Net.Extensions.Hosting
```

## Quick start

```csharp
using Core;

using var client = new ErlcClient("your-server-api-key");

var server = await client.GetServerInfoAsync(
    players: true,
    staff: true,
    joinLogs: true,
    queue: true,
    killLogs: true,
    commandLogs: true,
    modCalls: true,
    emergencyCalls: true,
    vehicles: true);

Console.WriteLine($"{server.Name}: {server.CurrentPlayers}/{server.MaxPlayers}");

foreach (var player in server.Players)
{
    Console.WriteLine($"{player.Name} ({player.Id}) - {player.Team}");
}
```

## Sending commands

Use `RunCommandAsync` to send a raw ER:LC server command:

```csharp
await client.RunCommandAsync(":h Server restart in 5 minutes");
```

Several models also expose convenience methods after they are returned by `ErlcClient`:

```csharp
var server = await client.GetServerInfoAsync(players: true, joinLogs: true);

var player = server.Players.FirstOrDefault();
if (player is not null)
{
    await player.Kick("Please rejoin the server.");
}

var latestJoin = server.JoinLogs.FirstOrDefault();
if (latestJoin is not null)
{
    await latestJoin.Ban("Rule violation");
}
```

## Change events

`ErlcClient` polls server information every 15 seconds and raises events when values change.

```csharp
using Core;

using var client = new ErlcClient("your-server-api-key");

client.PlayersChanged += (_, args) =>
{
    Console.WriteLine($"Player count changed: {args.Current.Count}");
};

client.ServerDetailsChanged += (_, args) =>
{
    Console.WriteLine($"Server is now {args.CurrentServerInfo.CurrentPlayers}/{args.CurrentServerInfo.MaxPlayers}");
};
```

Available events include:

- `ServerDetailsChanged`
- `PlayersChanged`
- `StaffChanged`
- `AdminsChanged`
- `ModsChanged`
- `HelpersChanged`
- `JoinLogsChanged`
- `QueueChanged`
- `KillLogsChanged`
- `CommandLogsChanged`
- `ModCallsChanged`
- `EmergencyCallsChanged`
- `VehiclesChanged`

Dispose the client when it is no longer needed to stop background polling and release the underlying `HttpClient`.

## Dependency injection

Add `Erlc.Net.Extensions.Hosting` and register `Core.ErlcClient` with your service collection.

### Register with an API key

```csharp
using Extensions.Hosting;

builder.Services.AddErlcClient("your-server-api-key");
```

### Register with configuration

```csharp
using Extensions.Hosting;

builder.Services.AddErlcClient(builder.Configuration.GetSection("Erlc"));
```

Example configuration:

```json
{
  "Erlc": {
    "ApiKey": "your-server-api-key"
  }
}
```

### Register with options

```csharp
using Extensions.Hosting;

builder.Services.AddErlcClient(options =>
{
    options.ApiKey = "your-server-api-key";
});
```

Then inject the client where needed:

```csharp
using Core;

public class ServerStatusService(ErlcClient client)
{
    public async Task PrintStatusAsync()
    {
        var server = await client.GetServerInfoAsync(players: true);
        Console.WriteLine($"{server.Name}: {server.CurrentPlayers}/{server.MaxPlayers}");
    }
}
```

## API coverage

`GetServerInfoAsync` accepts nullable boolean flags for each optional ER:LC response section:

```csharp
Task<ServerInfo> GetServerInfoAsync(
    bool? players = null,
    bool? staff = null,
    bool? joinLogs = null,
    bool? queue = null,
    bool? killLogs = null,
    bool? commandLogs = null,
    bool? modCalls = null,
    bool? emergencyCalls = null,
    bool? vehicles = null)
```

Set a flag to `true` to request that section, `false` to explicitly exclude it, or leave it as `null` to omit the query parameter.

## Authentication

ER:LC API requests require a private server API key. `ErlcClient` sends the key with the `Server-Key` header. Keep this value secret and avoid committing it to source control.

## Development

Restore and build the solution with the .NET SDK:

```bash
dotnet restore
dotnet build
```

The solution contains:

- `Core` - the main client and model library.
- `Extensions.Hosting` - dependency injection registration helpers for hosted applications.