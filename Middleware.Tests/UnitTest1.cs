using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using CustomMiddleware.Middleware;
using Microsoft.AspNetCore.Http;

namespace Middleware.Tests; 

    public class UnitTest1 : IAsyncLifetime {

        IHost? host;
        public Task DisposeAsync() {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync() {
            host = await new HostBuilder()
                .ConfigureWebHost(webBuilder => {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services => { })
                        .Configure(app => {
                            app.UseMiddleware<MyMiddleware>();
                            app.Run(async context =>
                            {
                            await context.Response.WriteAsync("Authorized.");
                            });
                        });
                })
                .StartAsync();
        }

        [Fact]
        public async Task TestNoCredentials(){
            var response = await host.GetTestClient().GetAsync("/");
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestCorrectCredentials() {
            var response = await host.GetTestClient().GetAsync("/?username=user1&password=password1");
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Authorized.", result);
        }

        [Fact]
        public async Task TestOnlyUsername() {
            var response = await host.GetTestClient().GetAsync("/?username=user1");
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

        [Fact]
        public async Task TestIncorrectCredentials() {
            var response = await host.GetTestClient().GetAsync("/?username=user5&password=password2");
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Not authorized.", result);
        }

}


