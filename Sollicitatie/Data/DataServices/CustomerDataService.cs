using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.State;
using Infrastructure;

namespace Data.DataServices {
  public interface ICustomerDataService {
    ValueTask<bool> CustomerExists(Guid id);
  }

  public class CustomerDataService : ICustomerDataService {
    private readonly IDbContext _dbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public CustomerDataService(IDbContext dbContext, ICurrentTenantProvider currentTenantProvider, ICosmosLinqQuery cosmosLinqQuery) {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
      _cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
    }

    public ValueTask<bool> CustomerExists(Guid id) {
      var query = _dbContext.Customers
        .GetItemLinqQueryable<CustomerState>()
        .Where(_ => _.TenantId == _currentTenantProvider.Get() && _.Id == id);

      return _cosmosLinqQuery.GetFeedIterator(query).ToAsyncEnumerable().AnyAsync();
    }
  }
}