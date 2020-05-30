using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data;
using Data.DataServices;
using Domain.Customers.State;
using Domain.Invoices.State;
using FakeItEasy;
using Infrastructure;
using Tests.TestingUtilities.CosmosDbTestingUtilities;
using Xunit;

namespace Tests.DataServices {
  public class InvoiceDataServiceTests {
    private readonly IDbContext _dbContext;
    private readonly InvoiceDataService _sut;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly Fixture _fixture;

    public InvoiceDataServiceTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new InvoiceDataService(_dbContext, _currentTenantProvider, new DummyLinqQuery());
      _fixture = new Fixture();
    }

    [Theory, AutoData]
    public async Task GIVEN_no_invoice_with_id_WHEN_InvoiceExists_THEN_return_false(Guid invoiceId) {
      A.CallTo(() => _dbContext.Invoices).Returns(ContainerFactory.FakeContainer(new InvoiceState[0]));

      var invoiceExists = await _sut.InvoiceExists(invoiceId);
      
      Assert.False(invoiceExists);
    }
    
    [Fact]
    public async Task GIVEN_invoice_with_id_WHEN_InvoiceExists_THEN_return_false() {
      var expectedInvoiceState = _fixture.Build<InvoiceState>()
        .With(_ => _.TenantId, _currentTenantProvider.Get())
        .Create();
      A.CallTo(() => _dbContext.Invoices).Returns(ContainerFactory.FakeContainer(new []{expectedInvoiceState}));

      var invoiceExists = await _sut.InvoiceExists(expectedInvoiceState.Id);
      
      Assert.True(invoiceExists);
    }
  }
}