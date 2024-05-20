namespace BIManagement.Modules.DataIntegration.SqlViewGenerator;

using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using JsonModel;
using SqlViewGenerating;

public static class SqlViewGenerator
{
    public static string GenerateView(EntityMapping mapping)
    {
        SqlViewVisitor visitor = new();
        mapping.Accept(visitor);
        return visitor.GetSqlView();
    }
}
