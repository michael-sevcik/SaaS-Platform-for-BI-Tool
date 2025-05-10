using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Persistence.Options;
using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Infrastructure.Identity;
using BIManagement.Modules.Users.Pages.Account;
using BIManagement.Modules.Users.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using BIManagement.Common.Persistence.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BIManagement.Modules.Users.Infrastructure.ServiceInstallers
{
    internal class IdentityServiceInstaller : IServiceInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            var keycloackConfig = configuration.GetSection("Modules:Users:Keycloak");

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "Keycloak", options =>
                {
                    options.Authority = keycloackConfig["Authority"];
                    options.ClientId = keycloackConfig["ClientId"];
                    options.ClientSecret = keycloackConfig["ClientSecret"];
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.SaveTokens = true;
                    options.MetadataAddress = keycloackConfig["MetadataAddress"];
                    
                    // dev only
                    options.RequireHttpsMetadata = false;

                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.TokenValidationParameters.ValidIssuer = keycloackConfig["ValidIssuer"];


                    options.Events = new OpenIdConnectEvents
                    {
                        OnTokenValidated = context =>
                        {
                            // Add roles to the principal
                            if (context.Principal is not null) { 
                                var claimsIdentity = context.Principal.Identity;
                                var accessToken = context.SecurityToken;
                                if (accessToken != null)
                                {
                                    var roles = accessToken.Claims.Where(c => c.Type == "role");

                                    foreach (var role in roles)
                                    {
                                        context.Principal.Identities.First().AddClaim(new Claim(ClaimTypes.Role, role.Value));
                                    }
                                }
                            }

                            return Task.CompletedTask;
                        },
                    };
                })
                .AddIdentityCookies();
                


            services.AddDbContext<UsersContext>((serviceProvider, options) => {
                ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()?.Value
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); ;
                options.UseSqlServer(
                    connectionString,
                    options => options.WithMigrationHistoryTableInSchema(UsersSchema.Name));
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender<ApplicationUser>, Identity.IdentityEmailSender>();
            services.AddHostedService<SeedingStartupTask>();
        }
    }
}
