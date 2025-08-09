using ExampleApi.Models;
using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Tests;

internal class GetJokeWithWireMock
{
    private WireMockManager _wireMockManager;

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
               .WithBody("{\"setup\":\"Why are pirates called pirates?\",\"punchline\":\"Because they arrr!\"}"));
        });
    }

    [Test]
    public async Task GetJokeFromMockedApi()
    {
        var response = await _wireMockManager.Client.GetAsync("/joke");
        var joke = await response.Content.ReadFromJsonAsync<Joke>();
        Assert.Multiple(() =>
        {
            response.EnsureSuccessStatusCode();
            Assert.That(joke?.Setup, Is.EqualTo("Why are pirates called pirates?"));
            Assert.That(joke?.Punchline, Is.EqualTo("Because they arrr!"));
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _wireMockManager.Dispose();
}