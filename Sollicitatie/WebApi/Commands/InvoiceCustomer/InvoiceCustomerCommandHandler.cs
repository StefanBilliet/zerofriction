using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Domain.Invoices;
using FluentValidation;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;
using InvoiceLine = Domain.Invoices.InvoiceLine;

namespace WebApi.Commands.InvoiceCustomer {
  public class InvoiceCustomerCommandHandler : ICommandHandler<InvoiceCustomerCommand> {
    private readonly IValidator<InvoiceCustomerCommand> _validator;
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceCustomerCommandHandler(IValidator<InvoiceCustomerCommand> validator, IInvoiceRepository invoiceRepository) {
      _validator = validator ?? throw new ArgumentNullException(nameof(validator));
      _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
    }

    public async Task Handle(InvoiceCustomerCommand command) {
      await _validator.ValidateAndThrowAsync(command);

      var invoiceLines = command.InvoiceLines.Select(_ => new InvoiceLine(_.Quantity, _.PricePerUnit, _.TotalAmount)).ToArray();
      var invoice = new Invoice(command.Id, command.Description, command.Date, command.CustomerId, command.TotalAmount, invoiceLines);

      await _invoiceRepository.Upsert(invoice);
    }
  }
}