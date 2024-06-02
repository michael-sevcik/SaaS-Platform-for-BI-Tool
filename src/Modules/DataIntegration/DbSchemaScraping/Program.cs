using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


ServiceCollection services = new();

// Add the EF Core services to the service collection
// Source: https://github.com/dotnet/efcore/issues/23595#issuecomment-740089427
var providerAssembly = Assembly.Load("Microsoft.EntityFrameworkCore.SqlServer");
var providerServicesAttribute = providerAssembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>();
var providerServicesType = providerAssembly.GetType(providerServicesAttribute.TypeName);
var providerServices = (IDesignTimeServices)(Activator.CreateInstance(providerServicesType) ?? throw new InvalidOperationException());
providerServices.ConfigureDesignTimeServices(services);

var sp = services.BuildServiceProvider();

var dbModelFactory = sp.GetRequiredService<IDatabaseModelFactory>();


var dbModel = dbModelFactory.Create(
    "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SaaSPlatform;" +
    "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server" +
    " Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
    new());

Console.WriteLine(dbModel.Tables.Count);

