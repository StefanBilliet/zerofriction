using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Domain.Invoices;
using FakeItEasy;
using WebApi.Commands.ChangeInvoiceStatus;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using WebApi.Commands.Shared;
using Xunit;

namespace Tests.Commands.ChangeInvoiceStatus {
  public class ChangeInvoiceStatusCommandHandlerTests {
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICommandHandler<ChangeInvoiceStatusCommand> _sut;
    private readonly Fixture _fixture;

    public ChangeInvoiceStatusCommandHandlerTests() {
      _invoiceRepository = A.Fake<IInvoiceRepository>();
      _sut = new ChangeInvoiceStatusCommandHandler(_invoiceRepository);
      _fixture = new Fixture();
    }

    [Theory]
    [InlineData(InvoiceStatus.Draft, InvoiceStatus.Sent)]
    [InlineData(InvoiceStatus.Sent, InvoiceStatus.PartiallyPaid)]
    [InlineData(InvoiceStatus.PartiallyPaid, InvoiceStatus.PaidInFull)]
    [InlineData(InvoiceStatus.PartiallyPaid, InvoiceStatus.Overdue)]
    [InlineData(InvoiceStatus.Draft, InvoiceStatus.Canceled)]
    public async Task GIVEN_draft_invoice_WHEN_changing_status_to_sent_THEN_changes_invoice_status_to_sent(InvoiceStatus oldStatus, InvoiceStatus newStatus) {
      var invoice = Invoice(oldStatus);
      A.CallTo(() => _invoiceRepository.Get(invoice.Id)).Returns(invoice);
      var command = new ChangeInvoiceStatusCommand {
        Id = invoice.Id,
        Status = newStatus
      };

      await _sut.Handle(command);
      
      Assert.Equal(newStatus, invoice.Deflate().Status);
      A.CallTo(() => _invoiceRepository.Upsert(invoice)).MustHaveHappened();
    }
    
    private static Invoice Invoice(Domain.Invoices.InvoiceStatus status) {
      return new Invoice(
        new Guid("6109AA78-2FE6-4519-ABBA-D0BAF6100DCE"),
        "Periodic invoice",
        new DateTimeOffset(new DateTime(2020, 01, 01)),
        new Guid("86D8629E-3E1E-4E35-9B1F-A1E591376CB6"),
        50,
        new[] {
          new InvoiceLine(1, 50, 50),
        },
        status
      );
    }
  }
}