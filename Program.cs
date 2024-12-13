using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareTest.Components;
using SoftwareTest.Components.Account;
using SoftwareTest.Data;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using BlazorApp1.Codes;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<HashingHandler>();
builder.Services.AddSingleton<SymetricEncryptionHandler>();
builder.Services.AddSingleton<AsymmetricEncryptionHandler>();
builder.Services.AddControllers(); // Add this to your service registration


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\Keys"))  // Ensure the key directory is persistent and accessible
    .SetApplicationName("TodoApp");  // Unique application name for key isolation


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

// Retrieve connection strings from your configuration
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var mockDbConnectionString = builder.Configuration.GetConnectionString("MockDBConnection")
                               ?? throw new InvalidOperationException("Connection string 'MockDBConnection' not found.");

var todoDatabaseConnectionString = builder.Configuration.GetConnectionString("Tododatabase")
                               ?? throw new InvalidOperationException("Connection string 'Tododatabase' not found.");

// Configure Database Contexts based on the OS platform
if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnectionString));

    builder.Services.AddDbContext<TododatabaseContext>(options =>
        options.UseSqlServer(todoDatabaseConnectionString));
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(mockDbConnectionString));

    builder.Services.AddDbContext<TododatabaseContext>(options =>
        options.UseSqlite(todoDatabaseConnectionString));
}
else
{
    // Default configuration for other platforms
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(defaultConnectionString));

    builder.Services.AddDbContext<TododatabaseContext>(options =>
        options.UseSqlServer(todoDatabaseConnectionString));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity with password requirements
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8; // Set the minimum password length to 8
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy =>
    {
        policy.RequireRole("Admin");
    });
});

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Configure Kestrel for HTTPS
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ConfigureHttpsDefaults(httpsOptions =>
//    {
//        var certPath = builder.Configuration["Kestrel:Endpoints:HttpsInlineCertFile:Certificate:Path"];
//        var certPassword = builder.Configuration["Kestrel:Endpoints:HttpsInlineCertFile:Certificate:Password"];

//        if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(certPassword))
//        {
//            httpsOptions.ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
//        }
//    });
//});

//// Explicitly disable IIS integration to enforce Kestrel-only usage
//builder.WebHost.UseKestrel(); // Only use Kestrel

// Throw an exception if the app is running in IIS (optional, for enforcement)
if (builder.Environment.IsProduction() && builder.Environment.WebRootPath.Contains("IIS"))
{
    throw new InvalidOperationException("This application should only run with Kestrel. IIS is not allowed.");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // For production, you may want to adjust the HSTS value.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
