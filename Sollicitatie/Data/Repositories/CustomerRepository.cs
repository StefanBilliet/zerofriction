using System;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Microsoft.Azure.Cosmos;

namespace Data.Repositories {
  public interface ICustomerRepository {
    Task Upsert(Customer customer);
  }
  
  public class CustomerRepository : ICustomerRepository {
    private readonly IDbContext _context;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public CustomerRepository(IDbContext context, ICurrentTenantProvider currentTenantProvider) {
      _context = context ?? throw new ArgumentNullException(nameof(context));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
    }
    
    public Task Upsert(Customer customer) {
      var customerState = customer.Deflate();
      customerState.TenantId = _currentTenantProvider.Get();
      
      return _context.Customers.UpsertItemAsync(customerState, new PartitionKey(_currentTenantProvider.Get()));
    }
  }
}