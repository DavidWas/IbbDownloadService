using System.Reflection;
using IbbDownloadService.Components;
using IbbDownloadService.NugetModule;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
logger.Information("Starting web host");
builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddNugetModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()); });
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();
app.UseNugetModule();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();