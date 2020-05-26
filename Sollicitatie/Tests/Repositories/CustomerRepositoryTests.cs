using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Data;
using Data.Repositories;
using Domain;
using Domain.State;
using FakeItEasy;
using Infrastructure;
using Microsoft.Azure.Cosmos;
using Tests.Infrastructure;
using Xunit;

namespace Tests.Repositories {
  public class CustomerRepositoryTests {
    private readonly IDbContext _dbContext;
    private readonly CustomerRepository _sut;
    private readonly ICurrentTenantProvider _currentTenantProvider;

    public CustomerRepositoryTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new CustomerRepository(_dbContext, _currentTenantProvider);
    }

    [Theory, AutoData]
    public async Task WHEN_Upsert_THEN_saves_customer(Customer customer) {
      var expectedCustomerState = customer.Deflate();
      expectedCustomerState.TenantId = _currentTenantProvider.Get();
      
      await _sut.Upsert(customer);

      A.CallTo(() =>
          _dbContext.Customers.UpsertItemAsync(
            A<CustomerState>.That.MatchesObject(expectedCustomerState),
            A<PartitionKey>.That.MatchesObject(new PartitionKey(_currentTenantProvider.Get())),
            null,
            default
          )
        )
        .MustHaveHappened();
    }
  }
}