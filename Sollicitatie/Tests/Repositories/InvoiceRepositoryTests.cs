using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data;
using Data.Repositories;
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

    public InvoiceRepositoryTests() {
      _dbContext = A.Fake<IDbContext>();
      _currentTenantProvider = new CurrentTenantProvider();
      _sut = new InvoiceRepository(_dbContext, _currentTenantProvider, new DummyLinqQuery());
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