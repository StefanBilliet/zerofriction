using System;
using System.Threading.Tasks;
using Data.DataServices;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Controllers {
  [ApiController]
  [Route("/api")]
  public class InvoicesController : ControllerBase {
    private readonly ICustomerDataService _customerDataService;
    private readonly IInvoiceDataService _invoiceDataService;
    private readonly ICommandHandler<InvoiceCustomerCommand> _invoiceCustomerCommandHandler;
    private readonly ICommandHandler<ChangeInvoiceStatusCommand> _changeInvoiceStatusCommandHandler;

    public InvoicesController(ICustomerDataService customerDataService, IInvoiceDataService invoiceDataService, ICommandHandler<InvoiceCustomerCommand> invoiceCustomerCommandHandler, ICommandHandler<ChangeInvoiceStatusCommand> changeInvoiceStatusCommandHandler) {
      _customerDataService = customerDataService ?? throw new ArgumentNullException(nameof(customerDataService));
      _invoiceDataService = invoiceDataService ?? throw new ArgumentNullException(nameof(invoiceDataService));
      _invoiceCustomerCommandHandler = invoiceCustomerCommandHandler ?? throw new ArgumentNullException(nameof(invoiceCustomerCommandHandler));
      _changeInvoiceStatusCommandHandler = changeInvoiceStatusCommandHandler ?? throw new ArgumentNullException(nameof(changeInvoiceStatusCommandHandler));
    }

    [HttpPost("invoices")]
    public async Task<IActionResult> InvoiceCustomer(InvoiceCustomerCommand command) {
      if (!await _customerDataService.CustomerExists(command.CustomerId)) {
        return NotFound("Customer");
      }

      try {
        await _invoiceCustomerCommandHandler.Handle(command);

        return NoContent();
      }
      catch (ValidationException exception) {
        return BadRequest(exception.Errors);
      }
    }

    [HttpPut("invoices/{id}/status")]
    public async Task<IActionResult> ChangeInvoiceStatus(Guid id, ChangeInvoiceStatusCommand command) {
      if (!await _invoiceDataService.InvoiceExists(id)) {
        return NotFound("Invoice");
      }

      try {
        await _changeInvoiceStatusCommandHandler.Handle(command);

        return NoContent();
      }
      catch (IllegalInvoiceStatusTransitionException exception) {
        return BadRequest(exception.Message);
      }
    }
  }
}