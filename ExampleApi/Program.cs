using ExampleApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<JokesService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/joke", async (JokesService jokesApi) => await jokesApi.GetJokeAsync());

app.Run();