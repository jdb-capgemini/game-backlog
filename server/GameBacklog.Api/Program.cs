using System.Text.Json.Serialization;
using GameBacklog.Api.Configuration;
using GameBacklog.Api.Data;
using GameBacklog.Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString(
        "GameBacklogDatabase")
    ?? throw new InvalidOperationException(
        "The database connection string is missing.");

var googleClientId =
    builder.Configuration["Authentication:Google:ClientId"]
    ?? throw new InvalidOperationException(
        "The Google ClientId has not been configured.");

var googleClientSecret =
    builder.Configuration["Authentication:Google:ClientSecret"]
    ?? throw new InvalidOperationException(
        "The Google ClientSecret has not been configured.");


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultAuthenticateScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultSignInScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;

        // Important: API authorization failures use the cookie handler.
        // Do not make Google the default challenge scheme.
        options.DefaultChallengeScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(
        CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.Cookie.Name = "game-backlog-auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy =
                CookieSecurePolicy.Always;
            options.Cookie.SameSite =
                SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            
            options.Cookie.IsEssential = true;

            options.ExpireTimeSpan =
                TimeSpan.FromDays(7);

            options.SlidingExpiration = true;

            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode =
                    StatusCodes.Status403Forbidden;

                return Task.CompletedTask;
            };
        })
    .AddGoogle(
        GoogleDefaults.AuthenticationScheme,
        options =>
        {
            options.ClientId =
                builder.Configuration[
                    "Authentication:Google:ClientId"]
                ?? throw new InvalidOperationException(
                    "Google ClientId is missing.");

            options.ClientSecret =
                builder.Configuration[
                    "Authentication:Google:ClientSecret"]
                ?? throw new InvalidOperationException(
                    "Google ClientSecret is missing.");

            options.CallbackPath = "/signin-google";
            options.SaveTokens = false;
        });

builder.Services.AddAuthorization();
        


builder.Services.AddDbContext<GameBacklogDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services
    .AddOptions<RawgOptions>()
    .Bind(builder.Configuration.GetSection(
        RawgOptions.SectionName))
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.BaseUrl),
        "The RAWG base URL is required.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ApiKey),
        "The RAWG API key is required.")
    .ValidateOnStart();

builder.Services.AddHttpClient<IRawgClient, RawgClient>(
    (serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<
                Microsoft.Extensions.Options.IOptions<RawgOptions>>()
            .Value;

        client.BaseAddress = new Uri(options.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(15);

        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "GameBacklogLearningApp/1.0");
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

var frontendBaseUrl =
    builder.Configuration["Frontend:BaseUrl"]
    ?? throw new InvalidOperationException(
        "The Frontend BaseUrl is missing.");

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueDevelopment", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", frontendBaseUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("VueDevelopment");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();