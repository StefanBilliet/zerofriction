using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands;
using WebApi.Commands.CreateCustomer.Contracts;

namespace WebApi.Controllers {
  [ApiController]
  [Route("/api")]
  public class CustomersController : ControllerBase {
    private readonly ICommandHandler<AddCustomerCommand> _addCustomerCommandHandler;

    public CustomersController(ICommandHandler<AddCustomerCommand> addCustomerCommandHandler) {
      _addCustomerCommandHandler = addCustomerCommandHandler ?? throw new ArgumentNullException(nameof(addCustomerCommandHandler));
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
  }
}