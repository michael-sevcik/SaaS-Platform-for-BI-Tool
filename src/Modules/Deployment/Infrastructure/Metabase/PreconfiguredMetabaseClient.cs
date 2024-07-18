using BIManagement.Common.Shared.Errors;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using BIManagement.Modules.Deployment.Domain.Configuration;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

[assembly: InternalsVisibleTo("BIManagement.Test.Modules.Deployment.Infrastructure")]

namespace BIManagement.Modules.Deployment.Infrastructure.Metabase;

/// <summary>
/// Default implementation of the <see cref="IPreconfiguredMetabaseClient"/>.
/// Configures metabase using the Metabase API - <see href="https://www.metabase.com/docs/latest/api-documentation"/>.
/// </summary>
sealed class PreconfiguredMetabaseClient : IPreconfiguredMetabaseClient
{
    private const string MetabaseApiUrlPath = "/api";
    private const string ErrorGroup = "Deployment.PreconfiguredMetabaseClient";
    private const string MetabaseApiKey = "mb_Uy19tAPolT6d1l+gtbpsjUJudirKjJGXMTndLPe6XZk=";
    private readonly HttpClient httpClient;
    private bool disposedValue;
    private readonly string metabaseRootApiUrl;

    /// <inheritdoc/>
    public PreconfiguredMetabaseClient(string metabaseRootUrl)
    {
        httpClient = new();
        httpClient.DefaultRequestHeaders.Add("x-api-key", MetabaseApiKey);
        metabaseRootApiUrl = metabaseRootUrl + MetabaseApiUrlPath;
    }

    /// <inheritdoc/>
    public async Task<Result> ChangeDefaultAdminEmail(string email)
    {
        var jsonBody = """
            {
              "email":"email",
              "first_name": "Admin",
              "is_group_manager": false,
              "locale": "en_US",
              "user_group_memberships": [
                {
                  "id": 1,
                  "is_group_manager": true
                },
                {
                  "id": 2,
                  "is_group_manager": true
                }
              ],
              "is_superuser": true,
              "login_attributes": {},
              "last_name": "Admin"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;

        bodyNode["email"] = email;

        return MapResponseToStatusCode(
            await httpClient.PutAsync(CreateAbsoluteApiUrl("/user/1"), JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminEmailFailed", "Failed to change default admin email."));
    }

    /// <inheritdoc/>
    public async Task<Result> ChangeDefaultAdminPassword(string password)
    {
        var jsonBody = """
            {
              "password": "string"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;

        bodyNode["password"] = password;

        return MapResponseToStatusCode(
            await httpClient.PutAsync(CreateAbsoluteApiUrl("/user/1/password"), JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminPasswordFailed", "Failed to change default admin password."));
    }

    /// <inheritdoc/>
    public async Task<Result> ConfigureDatabaseAsync(DatabaseSettings databaseSettings)
    {
        if (databaseSettings.Provider != DatabaseProvider.SqlServer)
        {
            throw new NotSupportedException();
        }

        var jsonBody = """
            {
              "is_on_demand":false,
              "is_full_sync":true,
              "is_sample":false,
              "cache_ttl":null,
              "refingerprint":null,
              "auto_run_queries":true,
              "schedules":{
                "metadata_sync":{
                  "schedule_minute":25,
                  "schedule_day":null,
                  "schedule_frame":null,
                  "schedule_hour":null,
                  "schedule_type":"hourly"
                },
                "cache_field_values":{
                  "schedule_minute":0,
                  "schedule_day":null,
                  "schedule_frame":null,
                  "schedule_hour":7,
                  "schedule_type":"daily"
                }
              },
              "details":{
                "host":"host.docker.internal",
                "port":32768,
                "db":"CostumerExampleData2",
                "instance":null,
                "user":"sa",
                "password":"**MetabasePass**",
                "ssl":true,
                "rowcount-override":null,
                "tunnel-enabled":false,
                "advanced-options":true,
                "additional-options":"trustServerCertificate=true",
                "let-user-control-scheduling":false
              },
              "name":"SQL server",
              "engine":"sqlserver"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;
        var detailsNode = bodyNode["details"]!;

        detailsNode["host"] = databaseSettings.Host;
        detailsNode["db"] = databaseSettings.DatabaseName;
        detailsNode["port"] = databaseSettings.Port;
        detailsNode["user"] = databaseSettings.Username;
        detailsNode["password"] = databaseSettings.Password;

        return MapResponseToStatusCode(
            await httpClient.PutAsync(CreateAbsoluteApiUrl("/database/2"), JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ConfiguringMetabaseFailed", "Failed to configure database."));
    }

    /// <inheritdoc/>
    public async Task<Result> ConfigureSmtpAsync(SmtpSettings smtpConfiguration)
    {
        var jsonBody = """
            {
              "email-smtp-security":"ssl", 
              "email-smtp-username":"example2@example.com",
              "email-smtp-password":"",
              "email-smtp-host":"host.docker.internal",
              "email-smtp-port":"587"
            }
            """;

        var jsonBodyWithoutSecurity = """
            {
              "email-smtp-security":"ssl",
              "email-smtp-host":"host.docker.internal",
              "email-smtp-port":"587"
            }
            """;

        string security = smtpConfiguration.Security switch
        {
            SmtpSecurity.None => "none",
            SmtpSecurity.Ssl => "ssl",
            SmtpSecurity.Tls => "tls",
            SmtpSecurity.StartTls => "starttsl",
            _ => throw new NotSupportedException()
        };

        var bodyNode = JsonNode.Parse(jsonBody)!;
        if (smtpConfiguration.Username == string.Empty && smtpConfiguration.Password == string.Empty)
        {
            bodyNode = JsonNode.Parse(jsonBodyWithoutSecurity)!;
        }
        else
        {
            bodyNode["email-smtp-username"] = smtpConfiguration.Username;
            bodyNode["email-smtp-password"] = smtpConfiguration.Password;
        }

        bodyNode["email-smtp-security"] = security;
        bodyNode["email-smtp-host"] = smtpConfiguration.Host;
        bodyNode["email-smtp-port"] = smtpConfiguration.Port.ToString();

        return MapResponseToStatusCode(
            await httpClient.PutAsync(CreateAbsoluteApiUrl("/email"), JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ConfiguringSmtpFailed", "Failed to configure smtp settings."));
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteDefaultTokenAsync() => MapResponseToStatusCode(
            await httpClient.DeleteAsync(CreateAbsoluteApiUrl("/api-key/1")),
            new Error(ErrorGroup + ".DeletionOfDefaultTokenFailed", "Failed to delete the default api token."));

    private static Result MapResponseToStatusCode(HttpResponseMessage response, Error error)
        => response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(error);

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                httpClient.Dispose();
            }

            disposedValue=true;
        }
    }

    private string CreateAbsoluteApiUrl(string relativeUrl) => metabaseRootApiUrl + relativeUrl;

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task<Result> WaitForMetabaseToStartResponding(int timeout = 300)
    {
        var startTime = DateTime.Now;
        var elapsedTime = TimeSpan.Zero;

        while (elapsedTime.TotalSeconds < timeout)
        {
            if ((await httpClient.GetAsync(CreateAbsoluteApiUrl("/user"))).IsSuccessStatusCode)
            {
                return Result.Success();
            }

            elapsedTime = DateTime.Now - startTime;
            await Task.Delay(3000); // Add a delay of 3 seconds
        }

        return Result.Failure(new Error(ErrorGroup + ".MetabaseNotResponding", "Metabase did not start responding within the specified timeout."));
    }
}
