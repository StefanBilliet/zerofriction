﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Invoices;
using Domain.Invoices.State;
using Infrastructure;
using Microsoft.Azure.Cosmos;

namespace Data.Repositories {
  public interface IInvoiceRepository {
    Task Upsert(Invoice invoice);
    Task<Invoice> Get(Guid id);
  }

  public class InvoiceRepository : IInvoiceRepository {
    private readonly IDbContext _dbContext;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICosmosLinqQuery _cosmosLinqQuery;

    public InvoiceRepository(IDbContext dbContext, ICurrentTenantProvider currentTenantProvider,
      ICosmosLinqQuery cosmosLinqQuery) {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
      _currentTenantProvider = currentTenantProvider ?? throw new ArgumentNullException(nameof(currentTenantProvider));
      _cosmosLinqQuery = cosmosLinqQuery ?? throw new ArgumentNullException(nameof(cosmosLinqQuery));
    }

    public Task Upsert(Invoice invoice) {
      var invoiceState = invoice.Deflate();
      invoiceState.TenantId = _currentTenantProvider.Get();

      return _dbContext.Invoices.UpsertItemAsync(invoiceState, new PartitionKey(_currentTenantProvider.Get()));
    }

    public async Task<Invoice> Get(Guid id) {
      var query = _dbContext.Invoices
        .GetItemLinqQueryable<InvoiceState>()
        .Where(_ => _.TenantId == _currentTenantProvider.Get() && _.Id == id);
      var invoiceState = await _cosmosLinqQuery.GetFeedIterator(query).ToAsyncEnumerable()
        .SingleOrDefaultAsync();

      if (invoiceState == null) {
        throw new AggregateNotFoundException<Invoice>();
      }

      var invoiceLines = invoiceState
        .InvoiceLines
        .Select(_ => new Domain.Invoices.InvoiceLine(_.Quantity, _.PricePerUnit, _.TotalAmount)).ToArray();
      return new Invoice(id, invoiceState.Description, invoiceState.Date, invoiceState.CustomerId,
        invoiceState.TotalAmount, invoiceLines, invoiceState.Status);
    }
  }
}