using ExampleApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Tests;

internal class GetJokeWithoutWireMock
{
    private WebApplicationFactory<Program> _factory = null!;

    [OneTimeSetUp]
    public void OneTimeSetup() => _factory = new WebApplicationFactory<Program>();

    [Test]
    public async Task GetJokeFromRealApi()
    {
        var httpClient = _factory.CreateClient();
        var response = await httpClient.GetAsync("/joke");

        response.EnsureSuccessStatusCode();
        var joke = response.Content.ReadFromJsonAsync<Joke>();
        Assert.That(joke, Is.Not.Null);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _factory.Dispose();
}