//using BIManagement.ManagementApp.Components.Account; // TODO:
//using BIManagement.ManagementApp.Data;
using BIManagement.ManagementApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.ManagementApp.Components.Layout;
using BIManagement.Common.Components.Layout;
using BIManagement.ManagementApp.StartupTasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<INavMenuContentProvider, NavMenuContentProvider>(); // TODO: use the service technique to provide the NavMenuContentProvider

builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Services.AddHostedService<MigrateDatabasesTask>();

Assembly[] infrastructureAssemblies = [
    BIManagement.Modules.DataIntegration.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Deployment.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Notifications.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Users.Infrastructure.AssemblyReference.Assembly
    ];

builder.Services.InstallModulesFromAssemblies(
    builder.Configuration,
    infrastructureAssemblies
);

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

app.AddModulesEndpointsFromAssemblies(infrastructureAssemblies);


var razorEndpointBuilder = app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        BIManagement.Modules.DataIntegration.Pages.AssemblyReference.Assembly,
        BIManagement.Modules.Deployment.Pages.AssemblyReference.Assembly,
        BIManagement.Modules.Users.Pages.AssemblyReference.Assembly
    );

if (builder.Environment.IsDevelopment()) 
{
    razorEndpointBuilder
        .AddAdditionalAssemblies(BIManagement.Modules.Notifications.Pages.AssemblyReference.Assembly);
}

app.Run();
