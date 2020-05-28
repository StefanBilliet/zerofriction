using System;
using System.Threading.Tasks;
using Domain.Invoices;
using Infrastructure;
using Microsoft.Azure.Cosmos;

namespace Data.Repositories {
  public interface IInvoiceRepository {
    Task Upsert(Invoice invoice);
  }

  public class InvoiceRepository : IInvoiceRepository {
    private readonly IDbContext _dbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public InvoiceRepository(IDbContext dbContext, ICurrentTenantProvider currentTenantProvider, ICosmosLinqQuery cosmosLinqQuery) {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
      _cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
    }

    public Task Upsert(Invoice invoice) {
      var invoiceState = invoice.Deflate();
      invoiceState.TenantId = _currentTenantProvider.Get();

      return _dbContext.Invoices.UpsertItemAsync(invoiceState, new PartitionKey(_currentTenantProvider.Get()));
    }
  }
}