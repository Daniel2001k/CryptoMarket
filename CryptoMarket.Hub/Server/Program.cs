using Okta.AspNetCore;
using Serilog;
using System.Reflection;

try
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddUserSecrets(Assembly.GetCallingAssembly())
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    Log.Information("Starting up");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.AddSerilog();

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddControllersWithViews();

    builder.Services.AddAuthentication(authOptions =>
    {
        authOptions.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
        authOptions.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
        authOptions.DefaultSignOutScheme = OktaDefaults.ApiAuthenticationScheme;
        authOptions.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    }).AddOktaWebApi(new OktaWebApiOptions()
    {
        OktaDomain = builder.Configuration.GetValue<string>("Okta:Domain")
    });

    var app = builder.Build();
    app.UseExceptionHandler("/Error");

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

    app.Run();

}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}