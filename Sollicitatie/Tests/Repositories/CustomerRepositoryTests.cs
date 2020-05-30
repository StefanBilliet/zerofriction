using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data;
using Data.Repositories;
using Domain.Customers;
using Domain.Customers.State;
using Domain.Exceptions;
using FakeItEasy;
using Infrastructure;
using Microsoft.Azure.Cosmos;
using Tests.TestingUtilities;
using Tests.TestingUtilities.CosmosDbTestingUtilities;
using Xunit;

namespace Tests.Repositories {
  public class CustomerRepositoryTests {
    private readonly IDbContext _dbContext;
    private readonly CustomerRepository _sut;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly Fixture _fixture;

    public CustomerRepositoryTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new CustomerRepository(_dbContext, _currentTenantProvider, new DummyLinqQuery());
      _fixture = new Fixture();
    }

    [Theory, AutoData]
    public async Task GIVEN_no_customer_with_id_WHEN_Get_THEN_throws(Guid customerId) {
      A.CallTo(() => _dbContext.Customers).Returns(ContainerFactory.FakeContainer(new CustomerState[0]));

      await Assert.ThrowsAsync<AggregateNotFoundException<Customer>>(() => _sut.Get(customerId));
    }
    
    [Fact]
    public async Task GIVEN_customer_with_id_WHEN_Get_THEN_returns_customer() {
      var expectedCustomerState = _fixture.Build<CustomerState>()
        .With(_ => _.TenantId, _currentTenantProvider.Get())
        .Create();
      A.CallTo(() => _dbContext.Customers).Returns(ContainerFactory.FakeContainer(new []{expectedCustomerState}));

      var customer = await _sut.Get(expectedCustomerState.Id);

      var actualCustomerState = customer.Deflate();
      actualCustomerState.TenantId = _currentTenantProvider.Get();
      AssertEx.Equal(expectedCustomerState, actualCustomerState);
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