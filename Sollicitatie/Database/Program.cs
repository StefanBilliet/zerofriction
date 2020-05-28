using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace Database {
  public static class Program {
    public static async Task Main(string[] args) {
      var client = new CosmosClientBuilder("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        .WithSerializerOptions(new CosmosSerializationOptions {PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase})
        .Build();

      await client.CreateDatabaseIfNotExistsAsync("zerofriction");
      var database = client.GetDatabase("zerofriction");
      await database.CreateContainerIfNotExistsAsync("customers", "/tenantId");
      await database.CreateContainerIfNotExistsAsync("invoices", "/tenantId");
    }
  }
}