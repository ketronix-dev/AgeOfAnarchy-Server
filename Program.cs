using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Server.ServiceClasses;
using Server.Contexts;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddAuthorization();

builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=server;Password=postgres"));

builder.Services.AddIdentityApiEndpoints<AuthUser>()
    .AddErrorDescriber<IdentityErrors>()
    .AddEntityFrameworkStores<Context>();

builder.Services.AddScoped<UserManager<AuthUser>>();

builder.Services.AddSingleton<IEmailSender<AuthUser>, DummyEmailSender>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");

app.UseCors(allowCORS);

app.MapIdentityApi<AuthUser>();
app.UseAuthorization();

app.Run();
