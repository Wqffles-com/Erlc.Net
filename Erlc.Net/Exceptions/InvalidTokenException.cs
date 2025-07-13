using System.Net;

namespace Erlc.Net.Exceptions;

/// <summary>
/// Represents an exception thrown when an invalid access token is detected
/// during an HTTP request.
/// </summary>
/// <remarks>
/// This exception is derived from <see cref="HttpRequestException"/> and is specifically
/// used to signal issues related to invalid authentication tokens.
/// </remarks>
/// <exception cref="InvalidTokenException">
/// Typically thrown when the HTTP response status code is 403 Forbidden,
/// indicating that the access token is invalid.
/// </exception>
public class InvalidTokenException : HttpRequestException
{
    public override string Message => "Invalid access token";

    public static void ThrowIfInvalid(HttpResponseMessage message)
    {
        if (message.StatusCode == HttpStatusCode.Forbidden)
            throw new InvalidTokenException();
    }
}