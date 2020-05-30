using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Customers;
using Domain.Customers.State;
using Domain.Exceptions;
using Infrastructure;
using Microsoft.Azure.Cosmos;
using Address = Domain.Customers.Address;
using ContactInformation = Domain.Customers.ContactInformation;
using ContactInformationType = Domain.Customers.ContactInformationType;

namespace Data.Repositories {
  public interface ICustomerRepository {
    Task Upsert(Customer customer);
    Task<Customer> Get(Guid id);
  }

  public class CustomerRepository : ICustomerRepository {
    private readonly IDbContext _dbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public CustomerRepository(IDbContext context, ICurrentTenantProvider currentTenantProvider,
      ICosmosLinqQuery cosmosLinqQuery) {
      _dbContext = context ?? throw new ArgumentNullException(nameof(context));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
      _cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
    }

    public Task Upsert(Customer customer) {
      var customerState = customer.Deflate();
      customerState.TenantId = _currentTenantProvider.Get();

      return _dbContext.Customers.UpsertItemAsync(customerState, new PartitionKey(_currentTenantProvider.Get()));
    }

    public async Task<Customer> Get(Guid id) {
      var query = _dbContext.Customers
        .GetItemLinqQueryable<CustomerState>()
        .Where(_ => _.TenantId == _currentTenantProvider.Get() && _.Id == id);
      var customerState = await _cosmosLinqQuery.GetFeedIterator(query).ToAsyncEnumerable()
        .SingleOrDefaultAsync();

      if (customerState == null) {
        throw new AggregateNotFoundException<Customer>();
      }

      var name = new Name(customerState.FirstName, customerState.SurName);
      var address = new Address(
        customerState.Address.Street,
        customerState.Address.NumberAndSuffix,
        customerState.Address.City,
        customerState.Address.AreaCode,
        customerState.Address.Area
      );
      var contactDetails = new ContactDetails(
        customerState.ContactDetails
          .Select(_ => new ContactInformation((ContactInformationType) _.Type, _.Value)).ToArray()
      );
      return new Customer(id, name, address, contactDetails);
    }
  }
}