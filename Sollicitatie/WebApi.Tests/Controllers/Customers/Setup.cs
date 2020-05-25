using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApi.CommandHandlers;
using WebApi.Commands.CreateCustomer;

namespace WebApi.Tests.Controllers.Customers {
  public partial class CustomersControllerTests {
    private readonly ICommandHandler<CreateCustomerCommand> _createCustomerCommandHandler;
    private readonly CustomersController _sut;

    public CustomersControllerTests() {
      _createCustomerCommandHandler = A.Fake<ICommandHandler<CreateCustomerCommand>>();
      _sut = new CustomersController(_createCustomerCommandHandler);
    }
  }

  [ApiController]
  [Route("/api")]
  public class CustomersController : ControllerBase {
    private readonly ICommandHandler<CreateCustomerCommand> _createCustomerCommandHandler;

    public CustomersController(ICommandHandler<CreateCustomerCommand> createCustomerCommandHandler) {
      _createCustomerCommandHandler = createCustomerCommandHandler ?? throw new ArgumentNullException(nameof(createCustomerCommandHandler));
    }

    [HttpPost]
    [Route("/customers")]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command) {
      try {
        await _createCustomerCommandHandler.Handle(command);

        return NoContent();
      }
      catch (ValidationException exception) {
        return BadRequest(exception.Errors);
      }
    }
  }
}