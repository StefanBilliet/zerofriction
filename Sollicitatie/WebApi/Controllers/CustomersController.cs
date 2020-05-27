using System;
using System.Threading.Tasks;
using Data.DataServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.AddCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Controllers {
  [ApiController]
  [Route("/api")]
  public class CustomersController : ControllerBase {
    private readonly ICommandHandler<AddCustomerCommand> _addCustomerCommandHandler;
    private readonly ICommandHandler<AddContactDetailsForCustomerCommand> _addContactDetailsForCustomerCommandHandler;
    private readonly ICustomerDataService _customerDataService;

    //to avoid injecting all these command handlers, in a production environment I would use MediatR and just inject IMedatior.
    public CustomersController(ICommandHandler<AddCustomerCommand> addCustomerCommandHandler,
      ICommandHandler<AddContactDetailsForCustomerCommand> addContactDetailsForCustomerCommandHandler,
      ICustomerDataService customerDataService) {
      _addCustomerCommandHandler = addCustomerCommandHandler ?? throw new ArgumentNullException(nameof(addCustomerCommandHandler));
      _addContactDetailsForCustomerCommandHandler = addContactDetailsForCustomerCommandHandler ?? throw new ArgumentNullException(nameof(addContactDetailsForCustomerCommandHandler));
      _customerDataService = customerDataService ?? throw new ArgumentNullException(nameof(customerDataService));
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer(AddCustomerCommand command) {
      try {
        await _addCustomerCommandHandler.Handle(command);

        return NoContent();
      }
      catch (ValidationException exception) {
        return BadRequest(exception.Errors);
      }
    }

    [HttpPost("customers/{id}/contactdetails")]
    public async Task<IActionResult> AddContactDetailsForCustomer(Guid id, AddContactDetailsForCustomerCommand command) {
      if (!await _customerDataService.CustomerExists(id)) {
        return new NotFoundObjectResult("Customer");
      }

      try {
        await _addContactDetailsForCustomerCommandHandler.Handle(command);

        return NoContent();
      }
      catch (ValidationException exception) {
        return BadRequest(exception.Errors);
      }
    }
  }
}