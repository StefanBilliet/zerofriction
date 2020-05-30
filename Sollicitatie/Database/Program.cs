using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace Database {
  public static class Program {
    public static async Task Main(string[] args) {
      var configurationRoot = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
      
      var client = new CosmosClientBuilder(configurationRoot["ZeroFrictionDatabaseConnectionString"])
        .WithSerializerOptions(new CosmosSerializationOptions {PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase})
        .Build();

      await client.CreateDatabaseIfNotExistsAsync("zerofriction");
      var database = client.GetDatabase("zerofriction");
      await database.CreateContainerIfNotExistsAsync("customers", "/tenantId");
      await database.CreateContainerIfNotExistsAsync("invoices", "/tenantId");
    }
  }
}