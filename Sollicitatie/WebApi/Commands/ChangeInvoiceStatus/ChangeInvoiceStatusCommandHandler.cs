using System;
using System.Threading.Tasks;
using Data.Repositories;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Commands.ChangeInvoiceStatus {
  public class ChangeInvoiceStatusCommandHandler : ICommandHandler<ChangeInvoiceStatusCommand> {
    private readonly IInvoiceRepository _invoiceRepository;

    public ChangeInvoiceStatusCommandHandler(IInvoiceRepository invoiceRepository) {
      _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
    }

    public async Task Handle(ChangeInvoiceStatusCommand command) {
      var invoice = await _invoiceRepository.Get(command.Id);

      switch (command.Status) {
        case InvoiceStatus.Sent:
          invoice.MarkAsSent();
          break;
        case InvoiceStatus.PartiallyPaid:
          invoice.MarkAsPartiallyPaid();
          break;
        case InvoiceStatus.PaidInFull:
          invoice.MarkAsPaidInFull();
          break;
        case InvoiceStatus.Overdue:
          invoice.MarkAsOverdue();
          break;
        case InvoiceStatus.Canceled:
          invoice.Cancel();
          break;
      }

      await _invoiceRepository.Upsert(invoice);
    }
  }
}