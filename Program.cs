using System.Text.Json;
using Database;
using UserManagment;
using ConfigManagment;

var builder = WebApplication.CreateBuilder(args);

var allowCORS = "_allowCORS";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowCORS,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ConfigManager.LoadConfig();
DatabaseUtils.Init();

app.MapGet("/", () => "Hello World!");

app.MapPost("/register", async (HttpContext x) => {
    var user = await x.Request.ReadFromJsonAsync<User>();

    await UserManager.CreateUser(user.FirstName, user.LastName, user.Login, user.Password);
    return "User created";
});

app.MapPost("/login", async (HttpContext x) =>
{
    var credentials = await x.Request.ReadFromJsonAsync<Dictionary<string, string>>();

    Console.WriteLine(credentials["login"]);

    if (credentials == null ||
        !credentials.TryGetValue("login", out var login) ||
        !credentials.TryGetValue("password", out var password))
    {
        return Results.BadRequest("Invalid login request");
    }

    string? token = await UserManager.Login(login, password);

    if (token != null)
    {
        return Results.Content($"{{\n\"token\": \"{token}\"\n}}", "application/json");
    }
    return Results.BadRequest("Login failed");
});

app.MapGet("/user", async (HttpContext x) =>
{
    var token = x.Request.Headers["Authorization"];
    if (token.Count == 0)
    {
        return Results.BadRequest("No token provided");
    }
    Console.WriteLine(token[0]);
    var user = await UserManager.GetUser(token[0]);
    if (user == null)
    {
        return Results.BadRequest("Invalid token");
    }

    return Results.Content(JsonSerializer.Serialize(user, new JsonSerializerOptions{WriteIndented = true}), "application/json");
});



app.UseCors(allowCORS);

app.Run();
