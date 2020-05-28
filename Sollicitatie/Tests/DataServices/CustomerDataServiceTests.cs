using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data;
using Data.DataServices;
using Domain.State;
using FakeItEasy;
using Infrastructure;
using Tests.TestingUtilities.CosmosDbTestingUtilities;
using Xunit;

namespace Tests.DataServices {
  public class CustomerDataServiceTests {
    private readonly IDbContext _dbContext;
    private readonly CustomerDataService _sut;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly Fixture _fixture;

    public CustomerDataServiceTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new CustomerDataService(_dbContext, _currentTenantProvider, new DummyLinqQuery());
      _fixture = new Fixture();
    }

    [Theory, AutoData]
    public async Task GIVEN_no_customer_with_id_WHEN_CustomerExists_THEN_return_false(Guid customerId) {
      A.CallTo(() => _dbContext.Customers).Returns(ContainerFactory.FakeContainer(new CustomerState[0]));

      var customerExists = await _sut.CustomerExists(customerId);
      
      Assert.False(customerExists);
    }
    
    [Fact]
    public async Task GIVEN_customer_with_id_WHEN_CustomerExists_THEN_return_false() {
      var expectedCustomerState = _fixture.Build<CustomerState>()
        .With(_ => _.TenantId, _currentTenantProvider.Get())
        .Create();
      A.CallTo(() => _dbContext.Customers).Returns(ContainerFactory.FakeContainer(new []{expectedCustomerState}));

      var customerExists = await _sut.CustomerExists(expectedCustomerState.Id);
      
      Assert.True(customerExists);
    }
  }
}