using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateOptions =>
{
    rateOptions.AddFixedWindowLimiter("fixed", options => {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;

    });
});

var app = builder.Build();

//Configure the HTTP request application
app.UseRateLimiter();
app.MapReverseProxy();

app.Run();
