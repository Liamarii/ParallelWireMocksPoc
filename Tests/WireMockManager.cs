using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.Server;

namespace Tests;

internal sealed class WireMockManager : IDisposable
{
    private readonly WireMockServer _wireMock;
    public WebApplicationFactory<Program> Factory;
    public HttpClient Client;
    private static readonly SemaphoreSlim _semaphoreSlim = new(3);

    public WireMockManager(Action<WireMockServer> configureStub)
    {
        _semaphoreSlim.Wait();

        try
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

        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public void Dispose()
    {
        Client?.Dispose();
        Factory?.Dispose();
        _wireMock?.Stop();
        _wireMock?.Dispose();
    }
}