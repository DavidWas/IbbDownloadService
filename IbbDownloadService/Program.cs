using System.Reflection;
using IbbDownloadService.Components;
using IbbDownloadService.NugetModule;
using IbbDownloadService.NugetModule.Services;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
logger.Information("Starting web host");
builder.Services.AddSingleton<ILogger>(logger);
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .MinimumLevel.Information()
        .WriteTo.Console()
    );
List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
builder.Services.AddNugetModule(builder.Configuration, logger, mediatRAssemblies);
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray()); });
builder.Services.AddHttpClient();
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