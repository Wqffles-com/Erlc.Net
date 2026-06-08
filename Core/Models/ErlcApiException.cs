using System;
using Newtonsoft.Json;

namespace Core.Models;

public class ErlcErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("commandid")]
    public string? CommandId { get; set; }
}

public class ErlcApiException : Exception
{
    public int StatusCode { get; }
    public ErlcErrorResponse? ErrorResponse { get; }

    public ErlcApiException(int statusCode, ErlcErrorResponse? errorResponse) 
        : base(errorResponse?.Error ?? $"ER:LC API returned status code {statusCode}")
    {
        StatusCode = statusCode;
        ErrorResponse = errorResponse;
    }
}
