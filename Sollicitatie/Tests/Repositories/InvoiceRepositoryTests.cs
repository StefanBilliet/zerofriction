using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data;
using Data.Repositories;
using Domain.Exceptions;
using Domain.Invoices;
using Domain.Invoices.State;
using FakeItEasy;
using Infrastructure;
using Microsoft.Azure.Cosmos;
using Tests.TestingUtilities;
using Tests.TestingUtilities.CosmosDbTestingUtilities;
using Xunit;

namespace Tests.Repositories {
  public class InvoiceRepositoryTests {
    private readonly IDbContext _dbContext;
    private readonly InvoiceRepository _sut;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly Fixture _fixture;

    public InvoiceRepositoryTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new InvoiceRepository(_dbContext, _currentTenantProvider, new DummyLinqQuery());
      _fixture = new Fixture();
    }

    [Theory, AutoData]
    public async Task GIVEN_no_invoice_with_id_WHEN_Get_THEN_throws(Guid invoiceId) {
      A.CallTo(() => _dbContext.Invoices).Returns(ContainerFactory.FakeContainer(new InvoiceState[0]));

      await Assert.ThrowsAsync<AggregateNotFoundException<Invoice>>(() => _sut.Get(invoiceId));
    }
    
    [Fact]
    public async Task GIVEN_invoice_with_id_WHEN_Get_THEN_returns_invoice() {
      var expectedInvoiceState = _fixture.Build<InvoiceState>()
        .With(_ => _.TenantId, _currentTenantProvider.Get())
        .Create();
      A.CallTo(() => _dbContext.Invoices).Returns(ContainerFactory.FakeContainer(new []{expectedInvoiceState}));

      var invoice = await _sut.Get(expectedInvoiceState.Id);

      var actualInvoiceState = invoice.Deflate();
      actualInvoiceState.TenantId = _currentTenantProvider.Get();
      AssertEx.Equal(expectedInvoiceState, actualInvoiceState);
    }
   
    [Theory, AutoData]
    public async Task WHEN_Upsert_THEN_saves_invoice(Invoice invoice) {
      var expectedInvoiceState = invoice.Deflate();
      expectedInvoiceState.TenantId = _currentTenantProvider.Get();
      
      await _sut.Upsert(invoice);

      A.CallTo(() =>
          _dbContext.Invoices.UpsertItemAsync(
            A<InvoiceState>.That.MatchesObject(expectedInvoiceState),
            A<PartitionKey>.That.MatchesObject(new PartitionKey(_currentTenantProvider.Get())),
            null,
            default
          )
        )
        .MustHaveHappened();
    }
  }
}