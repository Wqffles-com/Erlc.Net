using Erlc.Net.Core.Models;
using Xunit;

namespace Erlc.Net.Tests;

public class ErlcApiExceptionTests
{
    [Fact]
    public void Constructor_SetsStatusCodeAndErrorResponse()
    {
        var error = new ErlcErrorResponse { Error = "Invalid API key", Code = "invalid_key" };
        var ex = new ErlcApiException(401, error);
        Assert.Equal(401, ex.StatusCode);
        Assert.Same(error, ex.ErrorResponse);
        Assert.Equal("Invalid API key", ex.Message);
    }

    [Fact]
    public void Constructor_FallbackMessage_WhenErrorResponseIsNull()
    {
        var ex = new ErlcApiException(500, null);
        Assert.Equal(500, ex.StatusCode);
        Assert.Null(ex.ErrorResponse);
        Assert.Equal("ER:LC API returned status code 500", ex.Message);
    }

    [Fact]
    public void Constructor_FallbackMessage_WhenErrorIsEmpty()
    {
        var error = new ErlcErrorResponse();
        var ex = new ErlcApiException(400, error);
        Assert.Equal("ER:LC API returned status code 400", ex.Message);
    }

    [Fact]
    public void ErlcApiException_IsSerializable()
    {
        var error = new ErlcErrorResponse { Error = "Bad request", Code = "bad_request", CommandId = "cmd1" };
        var ex = new ErlcApiException(400, error);
        Assert.NotNull(ex.ErrorResponse?.Code);
        Assert.NotNull(ex.ErrorResponse?.CommandId);
    }
}
