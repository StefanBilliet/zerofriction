using System;
using System.Threading.Tasks;
using Domain.State;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Xunit;

namespace WebApi.Tests {
  public class Scratchpad {
    [Fact(Skip = "Scratchpad")]
    public async Task FactMethodName() {
      var client = new CosmosClientBuilder("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        .WithSerializerOptions(new CosmosSerializationOptions {PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase})
        .Build();

      await client.CreateDatabaseIfNotExistsAsync("zerofriction");
      var database = client.GetDatabase("zerofriction");
      await database.CreateContainerIfNotExistsAsync("customers", "/tenantId");
      var customersContainer = database.GetContainer("customers");
      await customersContainer.UpsertItemAsync(new CustomerState {
        Id = Guid.NewGuid(),
        TenantId = "localhost",
        FirstName = "Mark",
        SurName = "Spencer",
        Address = new Address() {
          Street = "Korenmarkt",
          NumberAndSuffix = "2",
          Area = "Oost-Vlaanderen",
          AreaCode = "9000"
        },
        ContactDetails = new[] {
          new ContactInformation {
            Type = ContactInformationType.Email,
            Value = "joske@gmail.com"
          }
        }
      }, new PartitionKey("localhost"));
    }
  }
}