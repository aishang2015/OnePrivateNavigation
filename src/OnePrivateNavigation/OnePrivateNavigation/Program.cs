using OnePrivateNavigation.Auth;
using OnePrivateNavigation.Client.Pages;
using OnePrivateNavigation.Client.States;
using OnePrivateNavigation.Components;
using OnePrivateNavigation.Data;
using OnePrivateNavigation.Data.Entities;
using OnePrivateNavigation.Data.Helpers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using OnePrivateNavigation.Helpers;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.ResponseCompression;

Environment.CurrentDirectory = AppContext.BaseDirectory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddDbContext<OnePrivateNavigationDbContext>(options =>
{
    var dbPath = Path.Combine(Environment.CurrentDirectory, "DB");
    if (!Directory.Exists(dbPath))
    {
        Directory.CreateDirectory(dbPath);
    }
    options
        .UseSqlite("Data Source=DB\\OnePrivateNavigation.db")
        .UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            if (!context.Set<User>().Any())
            {
                var salt = Guid.NewGuid().ToString("N");
                context.Set<User>().Add(new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = HashHelper.ComputeSHA256("admin" + salt),
                    Email = "admin@example.com",
                    Salt = salt,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                });
                await context.SaveChangesAsync();
            }
        });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthenticationService>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<FaviconHelper>();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true;
});

#region client need

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<NavMenuState>();

#endregion

var app = builder.Build();

// 确保数据库已创建并初始化种子数据
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OnePrivateNavigationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseResponseCompression();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

var favicons = Path.Combine(Environment.CurrentDirectory, "favicons");
if (!Directory.Exists(favicons))
{
    Directory.CreateDirectory(favicons);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(favicons),
    RequestPath = "/favicons"
});

app.MapStaticAssets();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(OnePrivateNavigation.Client._Imports).Assembly)
    .AllowAnonymous();

app.Run();
