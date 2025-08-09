using ExampleApi.Models;

namespace ExampleApi.Services;

public sealed class JokesService(HttpClient httpClient)
{
    public async Task<Joke?> GetJokeAsync()
    {
        return await httpClient.GetFromJsonAsync<Joke>("/random_joke");
    }
}