using ExampleApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<JokesService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["JokesApiUrl"] ?? throw new NotImplementedException("JokesApiUrl"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/joke", async (JokesService jokesApi) => await jokesApi.GetJokeAsync());

app.Run();