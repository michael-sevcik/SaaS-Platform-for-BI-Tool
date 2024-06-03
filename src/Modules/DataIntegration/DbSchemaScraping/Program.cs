using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


ServiceCollection services = new();

// Add the EF Core services to the service collection
// Source: https://github.com/dotnet/efcore/issues/23595#issuecomment-740089427
var assemblyName = "Microsoft.EntityFrameworkCore.SqlServer";
var providerAssembly = Assembly.Load(assemblyName);
var providerServicesAttribute = providerAssembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>()
    ?? throw new InvalidOperationException(
        $"Assembly \"{assemblyName}\" is missing required attribute \"{nameof(DesignTimeProviderServicesAttribute)}\"," +
        "which serves for identification of design time service provider.");

var providerServicesType = providerAssembly.GetType(providerServicesAttribute.TypeName)
    ?? throw new InvalidOperationException($"Assembly \"{assemblyName}\" has to provide type " +
    $"that has \"{nameof(DesignTimeProviderServicesAttribute)}\".");

var providerServices = (IDesignTimeServices)(Activator.CreateInstance(providerServicesType)
    ?? throw new InvalidOperationException($"EF Core design services provider type \"{providerServicesType.Name}\" is not" +
    $" initializable through \"{nameof(Activator)}\"."));

providerServices.ConfigureDesignTimeServices(services);

var sp = services.BuildServiceProvider();

var dbModelFactory = sp.GetRequiredService<IDatabaseModelFactory>();


var dbModel = dbModelFactory.Create(
    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SaaSPlatform;" +
    "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server" +
    " Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
    new());

Console.WriteLine(dbModel.Tables.Count);

