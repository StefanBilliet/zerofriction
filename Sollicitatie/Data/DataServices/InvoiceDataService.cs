using System;
using System.Threading.Tasks;
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
      throw new NotImplementedException();
    }
  }
}