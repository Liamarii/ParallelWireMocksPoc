using ExampleApi.Models;
using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Tests;

[Parallelizable]
internal class GetAnotherJokeWithWireMock
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
               .WithBody("{\"setup\":\"How do you organize a space party?\",\"punchline\":\"Planet!\"}"));
        });
    }

    [Test]
    public async Task GetAnotherJokeFromMockedApi()
    {
        Thread.Sleep(TimeSpan.FromSeconds(3));
        var response = await _wireMockManager.Client.GetAsync("/joke");

        response.EnsureSuccessStatusCode();
        var joke = await response.Content.ReadFromJsonAsync<Joke>();
        Assert.Multiple(() =>
        {
            Assert.That(joke?.Setup, Is.EqualTo("How do you organize a space party?"));
            Assert.That(joke?.Punchline, Is.EqualTo("Planet!"));
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _wireMockManager.Dispose();
}