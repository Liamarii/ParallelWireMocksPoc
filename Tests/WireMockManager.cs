using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.Server;

namespace Tests;

internal class WireMockManager : IDisposable
{
    private readonly WireMockServer _wireMock;
    public WebApplicationFactory<Program> Factory;
    public HttpClient Client;

    public WireMockManager(Action<WireMockServer> configureStub)
    {
        _wireMock = WireMockServer.Start();
        configureStub(_wireMock);

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, conf) =>
                {
                    conf.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["JokesApiUrl"] = _wireMock.Urls.Single()
                    });
                });
            });

        Client = Factory.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        Factory?.Dispose();
        _wireMock?.Stop();
        _wireMock?.Dispose();
    }
}