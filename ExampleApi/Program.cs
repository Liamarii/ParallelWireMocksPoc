
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/users", () =>
{
    return new List<User>()
        {
            new("Anish", "Giri"),
            new("Bob","Fischer"),
            new("Carl","Magnuson")
        };
});

app.Run();


public record User(string forename, string surname);