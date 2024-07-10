using BIManagement.Common.Shared.Errors;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using BIManagement.Modules.Deployment.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BIManagement.Modules.Deployment.Infrastructure.Metabase;

/// <summary>
/// 
/// </summary>
class PreconfiguredMetabaseClient : IPreconfiguredMetabaseClient
{
    private const string MetabaseApiUrlPath = "/api";
    private const string ErrorGroup = "Deployment.PreconfiguredMetabaseClient";
    private readonly HttpClient httpClient = new HttpClient();

    /// <inheritdoc/>
    public PreconfiguredMetabaseClient(string metabaseRootUrl)
    {
        httpClient.DefaultRequestHeaders.Add("x-api-key", "mb_KgUepT6vWo98hIqukd6B/Ydix6noM9/v4Wip8GrOBx4=");
        httpClient.BaseAddress = new Uri(metabaseRootUrl + MetabaseApiUrlPath);
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
            await httpClient.PutAsync("/user/1", JsonContent.Create(bodyNode)),
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
            await httpClient.PutAsync("/user/1/password", JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminPasswordFailed", "Failed to change default admin password."));
    }

    /// <inheritdoc/>
    public async Task<Result> ConfigureDatabaseAsync(DatabaseSettings databaseSettings)
    {
        if (databaseSettings.Provider != DatabaseProvider.SqlServer)
        {
            throw new NotSupportedException();
        }

        // TODO: 
        var jsonBody = """
            {
              "password": "string"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;

        bodyNode["password"] = password;

        return MapResponseToStatusCode(
            await httpClient.PutAsync("/user/1/password", JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminPasswordFailed", "Failed to change default admin password."));
    }

    /// <inheritdoc/>
    public async Task<Result> ConfigureSmtpAsync(SmtpSettings smtpConfiguration)
    {
        // TODO: 

        var jsonBody = """
            {
              "password": "string"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;

        bodyNode["password"] = password;

        return MapResponseToStatusCode(
            await httpClient.PutAsync("/user/1/password", JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminPasswordFailed", "Failed to change default admin password."));
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteDefaultTokenAsync()
    {
        // TODO: 
        var jsonBody = """
            {
              "password": "string"
            }
            """;

        var bodyNode = JsonNode.Parse(jsonBody)!;

        bodyNode["password"] = password;

        return MapResponseToStatusCode(
            await httpClient.PutAsync("/user/1/password", JsonContent.Create(bodyNode)),
            new Error(ErrorGroup + ".ChangeOfDefaultAdminPasswordFailed", "Failed to change default admin password."));
    }

    private static Result MapResponseToStatusCode(HttpResponseMessage response, Error error)
    {
        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(error);
    }
}
