using System;
using System.Threading.Tasks;
using Data.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Controllers {
  [ApiController]
  [Route("/api")]
  public class InvoicesController : ControllerBase {
    private readonly ICustomerDataService _customerDataService;
    private readonly ICommandHandler<InvoiceCustomerCommand> _invoiceCustomerCommandHandler;

    public InvoicesController(ICustomerDataService customerDataService, ICommandHandler<InvoiceCustomerCommand> invoiceCustomerCommandHandler) {
      _customerDataService = customerDataService ?? throw new ArgumentNullException(nameof(customerDataService));
      _invoiceCustomerCommandHandler = invoiceCustomerCommandHandler ?? throw new ArgumentNullException(nameof(invoiceCustomerCommandHandler));
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
  }
}