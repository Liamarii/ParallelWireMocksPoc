using ExampleApi.Models;
using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Tests;

[Parallelizable]
internal sealed class GetJokeWithWireMock
{
    private WireMockManager _wireMockManager;
    private const string _setup = "Why are pirates called pirates?";
    private const string _punchline = "Because they arrr!";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _wireMockManager = new(server =>
        {
            server.Given(Request.Create()
               .WithPath("/random_joke")
               .UsingGet())
               .RespondWith(Response.Create()
               .WithStatusCode(200)
               .WithBody($"{{\"setup\":\"{_setup}\",\"punchline\":\"{_punchline}\"}}"));
        });
    }

    [Test]
    public async Task GetJokeFromMockedApi()
    {
        Thread.Sleep(TimeSpan.FromSeconds(3));
        var response = await _wireMockManager.Client.GetAsync("/joke");
        var joke = await response.Content.ReadFromJsonAsync<Joke>();
        
        Assert.Multiple(() =>
        {
            response.EnsureSuccessStatusCode();
            Assert.That(joke?.Setup, Is.EqualTo(_setup));
            Assert.That(joke?.Punchline, Is.EqualTo(_punchline));
        });
    }
}