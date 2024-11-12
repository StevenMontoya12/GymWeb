using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorFrontend.Services;
using BlazorFrontend;
using Blazored.LocalStorage;
using System.Net.Http;
using FrontEndGym;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Registrar Blazored.LocalStorage para almacenamiento local
builder.Services.AddBlazoredLocalStorage();

// Registrar HttpClient para conectar a la API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5085/")
});

// Registrar AuthService para manejar autenticación
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
