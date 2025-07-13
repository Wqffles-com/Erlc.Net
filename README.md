# Erlc.Net


Erlc.Net is a modern API wrapper for the popular Roblox game Emergency Response: Liberty County.

Erlc.Net makes use of modern C# features, including async/await via TAP, seperate types and use of System.Text.Json

## Getting Started

Firstly, make sure your server has the 'ER:LC API Pack', which can be bought for 600 Robux.

Next, find your API Key in your server's settings, which can be found in the top right.
After you have your API Key, you can make an ErlcClient to interact with Police Roleplay Community's API:
```csharp
using Erlc.Net;

var token = "<FILL-IN-TOKEN-HERE>";

var client = new ErlcClient(token);
```

To test if your API Key works, you can check your server's status:

```csharp
var response = await client.GetServer();
Console.WriteLine(response.StatusCode);
// Expected: 200
// 403 indicates a faulty API Key.
```

## Contributing

If you'd like to contribute, you're welcome to fork this project, change code and create a pull request to the dev branch.
A tutorial on this can be found with various sources, including YouTube and GitHub.

Thanks for contributing to my project!


<a href="https://dot.net"><img alt="Built with C#" src="https://badges.penpow.dev/badges/built-with/csharp/cozy.svg"></a>
<a href="https://coff.ee/wqffles.com"><img alt="Buy me a coffee" src="https://badges.penpow.dev/badges/donate/buy-me-a-coffee/cozy.svg"></a>