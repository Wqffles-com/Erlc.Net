using System.Net;

namespace Erlc.Net;

public struct ErlcResponse<TResult>
{
    public TResult? Result { get; set; }
    public bool Success { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public struct ErlcResponse
{
    public bool Success { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}