

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<JokesApiClient>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/joke", async (JokesApiClient jokesApi) => await jokesApi.GetJokeAsync());

app.Run();

public record Joke(string setup, string punchline);

public class JokesApiClient(HttpClient httpClient)
{
    public async Task<Joke?> GetJokeAsync()
    {
        return await httpClient.GetFromJsonAsync<Joke>("https://official-joke-api.appspot.com/random_joke");
    }
}