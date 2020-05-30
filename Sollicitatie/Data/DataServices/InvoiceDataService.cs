using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Invoices.State;
using Infrastructure;

namespace Data.DataServices {
  public interface IInvoiceDataService {
    ValueTask<bool> InvoiceExists(Guid id);
  }

  public class InvoiceDataService : IInvoiceDataService {
    private readonly IDbContext _dbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public InvoiceDataService(IDbContext dbContext, ICurrentTenantProvider currentTenantProvider, ICosmosLinqQuery cosmosLinqQuery) {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
      _cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
    }
    
    public ValueTask<bool> InvoiceExists(Guid id) {
      var query = _dbContext.Invoices
        .GetItemLinqQueryable<InvoiceState>()
        .Where(_ => _.TenantId == _currentTenantProvider.Get() && _.Id == id);

      return _cosmosLinqQuery.GetFeedIterator(query).ToAsyncEnumerable().AnyAsync();
    }
  }
}