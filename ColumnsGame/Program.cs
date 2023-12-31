using Blazored.LocalStorage;
using ColumnsGame;
using ColumnsGame.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddSingleton<IColumnGenerator, ColumnGenerator>();
builder.Services.AddTransient<IColumnFallManager, ColumnFallManager>();
builder.Services.AddSingleton<IGameArea, GameArea>();
builder.Services.AddSingleton<IHighScores, HighScores>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
