using BIManagement.Common.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.SchemaScraping;

public interface ISchemaScraper
{
    Task<Result<IReadOnlyList<Table>>> ScrapeSchemaAsync(DatabaseConnectionConfiguration configuration);
}
