using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Erlc.Net.Core;

public abstract class BaseEntity
{
    [JsonIgnore]
    internal ErlcClient? Client { get; set; }

    protected ErlcClient GetClient()
    {
        return Client ?? throw new InvalidOperationException("Client is not initialized for this entity.");
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        if (context.Context is ErlcClient client)
        {
            Client = client;
        }
    }
}
