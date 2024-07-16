using BIManagement.ManagementApp.Components;
using System.Reflection;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.ManagementApp.Components.Layout;
using BIManagement.Common.Components.Layout;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<INavMenuContentProvider, NavMenuContentProvider>();

Assembly[] infrastructureAssemblies = [
    BIManagement.Modules.DataIntegration.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Deployment.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Notifications.Infrastructure.AssemblyReference.Assembly,
    BIManagement.Modules.Users.Infrastructure.AssemblyReference.Assembly
    ];

builder.Services.InstallServicesFromAssemblies(
    builder.Configuration,
    BIManagement.ManagementApp.AssemblyReference.Assembly,
    BIManagement.Common.Persistence.AssemblyReference.Assembly
);

builder.Services.InstallModulesFromAssemblies(
    builder.Configuration,
    infrastructureAssemblies
);

var app = builder.Build();

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseAntiforgery();

app.AddModulesEndpointsFromAssemblies(infrastructureAssemblies);


var razorEndpointBuilder = app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        BIManagement.Modules.DataIntegration.Pages.AssemblyReference.Assembly,
        BIManagement.Modules.Deployment.Pages.AssemblyReference.Assembly,
        BIManagement.Modules.Users.Pages.AssemblyReference.Assembly
    );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    razorEndpointBuilder
        .AddAdditionalAssemblies(BIManagement.Modules.Notifications.Pages.AssemblyReference.Assembly);
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Run();
