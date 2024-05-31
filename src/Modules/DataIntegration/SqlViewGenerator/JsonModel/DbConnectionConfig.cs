namespace BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;

public class DbConnectionConfig
{
    public DbConnectionConfig(string databaseProvider, string server, string initialCatalog)
    {
        DatabaseProvider = databaseProvider;
        Server = server;
        InitialCatalog = initialCatalog;
    }

    public string DatabaseProvider { get; }

    public string Server { get; }

    public string InitialCatalog { get; }


}