using FluentMigrator.Runner;
using Radzen;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


using WEB_Beadandó.Client.Pages;
using WEB_Beadandó.Components;
using WEB_Beadandó.DataServices;
using WEB_Beadandó.EndPoints.Branch;
using WEB_Beadandó.EndPoints.Car    ;
using System;
using WEB_Beadandó.EndPoints.Brands;
using WEB_Beadandó.EndPoints.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Connection string beolvasása az appsettings.json-ból
var connectionString = builder.Configuration.GetConnectionString("DbConn");

// FluentMigrator beállítása
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSqlServer()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(CreateTables).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7216");
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddSingleton<IDataServices, DataServices>();

builder.Services.AddRadzenComponents();


var app = builder.Build();
// Migrációk futtatása induláskor
using (var scope = app.Services.CreateScope())
{
    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    migrator.MigrateUp();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//EndPointok hozzáadása
app.AddBranchEndpoints();
app.AddCarEndpoints();
app.AddBrandEndpoints();
app.AddTypeEndpoints();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WEB_Beadandó.Client._Imports).Assembly);

app.Run();
