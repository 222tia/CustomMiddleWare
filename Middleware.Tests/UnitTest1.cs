namespace Middleware.Tests;

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Web.Middleware;

namespace Middleware.Tests;

public class UnitTest1 {
    [Fact]
    public async Task MiddlewareTest_FailWhenNotAuthenticated()
    {
    using var host = await new HostBuilder()
    .ConfigureWebHost(webBuilder =>
    {
    webBuilder
    .UseTestServer()
    .ConfigureServices(services =>
    {
    })
    .Configure(app =>
    {
    app.UseMiddleware<MyAuth>();
    });
    })
    .StartAsync();
    var response = await host.GetTestClient().GetAsync("/");
    Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
    var result = await response.Content.ReadAsStringAsync();
    Assert.Equal("Failed!", result);
    }
}