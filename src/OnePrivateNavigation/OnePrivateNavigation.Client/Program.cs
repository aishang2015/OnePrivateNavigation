using OnePrivateNavigation.Client.States;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

Environment.CurrentDirectory = AppContext.BaseDirectory;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 200;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
    config.SnackbarConfiguration.PreventDuplicates = false;
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<NavMenuState>();

await builder.Build().RunAsync();
