namespace SqlViewGenerator.JsonModel;

public class DbConnectionConfig
{
    public DbConnectionConfig(string databaseProvider, string server, string initialCatalog)
    {
        this.DatabaseProvider = databaseProvider;
        this.Server = server;
        this.InitialCatalog = initialCatalog;
    }

    public string DatabaseProvider { get; }

    public string Server { get; }

    public string InitialCatalog { get; }


}