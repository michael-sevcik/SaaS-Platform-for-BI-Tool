using BIManagement.ManagementApp.Components.Account;
using BIManagement.ManagementApp.Data;
using BIManagement.ManagementApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.ManagementApp.Components.Layout;
using BIManagement.Common.Components.Layout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<INavMenuContentProvider, NavMenuContentProvider>(); // TODO: use the service technique to provide the NavMenuContentProvider

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// TODO: DELETE moved to users.ServiceInstallers.IdentitySrviceInstaller
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Install services and modules from the specified assemblies.
//builder.Services.InstallServicesFromAssemblies( // TODO: INSTALL SERVICES
//builder.Services.InstallModulesFromAssemblies(
//    builder.Configuration,
//    BIManagement.Modules.DataIntegration.Infrastructure.AssemblyReference.Assembly,
//    BIManagement.Modules.Deployment.Infrastructure.AssemblyReference.Assembly,
//    BIManagement.Modules.Notifications.Infrastructure.AssemblyReference.Assembly,
//    BIManagement.Modules.Users.Infrastructure.AssemblyReference.Assembly
//);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        BIManagement.Modules.DataIntegration.Pages.AssemblyReference.Assembly
        //BIManagement.Modules.DataIntegration.Pages.AssemblyReference.Assembly,
        //BIManagement.Modules.Deployment.Pages.AssemblyReference.Assembly,
        //BIManagement.Modules.Users.Pages.AssemblyReference.Assembly
    );

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
