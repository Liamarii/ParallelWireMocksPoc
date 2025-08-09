using ExampleApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<JokesService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["JokesApiUrl"] ?? throw new NotImplementedException("JokesApiUrl"));
});

builder.Services.AddOpenApi();

var app = builder.Build();
app.MapScalarApiReference(o => o.Theme = ScalarTheme.BluePlanet);

app.MapOpenApi();
app.UseHttpsRedirection();

app.MapGet("joke", async (JokesService jokesApi) => await jokesApi.GetJokeAsync());

app.Run();