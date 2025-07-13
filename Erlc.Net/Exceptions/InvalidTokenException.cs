using System.Net;

namespace Erlc.Net.Exceptions;

public class InvalidTokenException : HttpRequestException
{
    public override string Message => "Invalid access token";

    public static void ThrowIfInvalid(HttpResponseMessage message)
    {
        if (message.StatusCode == HttpStatusCode.Forbidden)
            throw new InvalidTokenException();
    }
}