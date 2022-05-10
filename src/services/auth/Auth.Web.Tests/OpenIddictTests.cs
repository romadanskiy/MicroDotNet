using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace AuthorizationServer.Web.Tests;

public class OpenIddictTests: IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _applicationFactory;

    public OpenIddictTests(CustomWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task RequestToken_ReturnsAccessToken()
    {
        Env.LoadFromCurrentDirectory();
        var client = _applicationFactory.CreateClient();
       
        var values = new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new("username", "Admin"),
            new("password", "qWe!123"),
        };
        var content = new FormUrlEncodedContent(values);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
        
        var response = await client.PostAsync("/connect/token", content);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var data = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(data?.AccessToken);
    }
}