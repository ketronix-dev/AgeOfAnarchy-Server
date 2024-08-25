using Appwrite;
using Appwrite.Models;
using Appwrite.Services;
using Crypto;
using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using UserManagment;

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

DatabaseUtils.Init();

app.MapGet("/", () => "Hello World!");

app.MapPost("/register", async (HttpContext x) => {

    // create a new user from data in body
    var user = await x.Request.ReadFromJsonAsync<User>();

    await UserManager.CreateUser(user.FirstName, user.LastName, user.Login, user.Password);
    return "User created";
});

app.MapPost("/login", async (HttpContext x) =>
{
    // Get login and password from body
    var credentials = await x.Request.ReadFromJsonAsync<Dictionary<string, string>>();

    Console.WriteLine(credentials["login"]);

    if (credentials == null ||
        !credentials.TryGetValue("login", out var login) ||
        !credentials.TryGetValue("password", out var password))
    {
        return Results.BadRequest("Invalid login request");
    }


    // login user
    string? token = await UserManager.Login(login, password);

    if (token != null)
    {
        return Results.Content($"{{\n\"token\": \"{token}\"\n}}", "application/json");
    }
    return Results.BadRequest("Login failed");
});



app.UseCors(allowCORS);

app.Run();
