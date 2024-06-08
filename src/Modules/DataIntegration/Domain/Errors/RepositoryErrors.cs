using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.DataIntegration.Domain.Errors;

public static class RepositoryErrors
{
    public const string Namespace = "DataIntegration";
    public static readonly Error DatabaseOperationFailed = new(
        $"{Namespace}.DatabaseOperationFailed",
        "Saving changes failed.");
}
