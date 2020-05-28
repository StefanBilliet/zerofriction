using System;
using Microsoft.Azure.Cosmos;

namespace Data {
  public interface IDbContext {
    public Container Customers { get; }
    public Container Invoices { get;  }
  }

  public class DbContext : IDbContext {
    private readonly CosmosClient _cosmosClient;

    public Container Customers => _cosmosClient.GetContainer(DatabaseInfo.Database, DatabaseInfo.CustomersContainer);
    public Container Invoices => _cosmosClient.GetContainer(DatabaseInfo.Database, DatabaseInfo.InvoicesContainer);

    public DbContext(CosmosClient cosmosClient) {
      _cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient));
    }
  }
}