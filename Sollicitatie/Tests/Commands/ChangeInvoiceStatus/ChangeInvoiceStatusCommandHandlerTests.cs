using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Data.Repositories;
using Domain.Invoices;
using FakeItEasy;
using Tests.TestingUtilities;
using WebApi.Commands.ChangeInvoiceStatus;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using WebApi.Commands.Shared;
using Xunit;
using InvoiceStatus = WebApi.Commands.ChangeInvoiceStatus.Contracts.InvoiceStatus;

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
    [InlineData(Domain.Invoices.InvoiceStatus.Draft, InvoiceStatus.Sent, Domain.Invoices.State.InvoiceStatus.Sent)]
    [InlineData(Domain.Invoices.InvoiceStatus.Sent, InvoiceStatus.PartiallyPaid, Domain.Invoices.State.InvoiceStatus.PartiallyPaid)]
    [InlineData(Domain.Invoices.InvoiceStatus.PartiallyPaid, InvoiceStatus.PaidInFull, Domain.Invoices.State.InvoiceStatus.PaidInFull)]
    [InlineData(Domain.Invoices.InvoiceStatus.PartiallyPaid, InvoiceStatus.Overdue, Domain.Invoices.State.InvoiceStatus.Overdue)]
    [InlineData(Domain.Invoices.InvoiceStatus.Draft, InvoiceStatus.Canceled, Domain.Invoices.State.InvoiceStatus.Canceled)]
    public async Task GIVEN_draft_invoice_WHEN_changing_status_to_sent_THEN_changes_invoice_status_to_sent(Domain.Invoices.InvoiceStatus oldStatus, InvoiceStatus newStatus, Domain.Invoices.State.InvoiceStatus expectedNewStatus) {
      var invoice = Invoice(oldStatus);
      A.CallTo(() => _invoiceRepository.Get(invoice.Id)).Returns(invoice);
      var command = new ChangeInvoiceStatusCommand {
        Id = invoice.Id,
        Status = newStatus
      };

      await _sut.Handle(command);
      
      Assert.Equal(expectedNewStatus, invoice.Deflate().Status);
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