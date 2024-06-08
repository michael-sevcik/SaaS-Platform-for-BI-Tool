using BIManagement.Common.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.Errors;

public static class RepositoryErrors
{
    public const string Namespace = "DataIntegration";
    public static readonly Error DatabaseOperationFailed = new(
        $"{Namespace}.DatabaseOperationFailed",
        "Saving changes failed.");
}
