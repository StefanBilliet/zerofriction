using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.CreateCustomer;
using Xunit;

namespace WebApi.Tests.Controllers.Customers {
  public partial class CustomersControllerTests {
    [Theory, AutoData]
    public async Task WHEN_POST_customer_with_invalid_command_THEN_returns_BadRequest(CreateCustomerCommand command) {
      var validationFailures = new []{new ValidationFailure(nameof(CreateCustomerCommand.FirstName), "required") };
      A.CallTo(() => _createCustomerCommandHandler.Handle(command)).Throws(new ValidationException(validationFailures));
      
      var result = await _sut.CreateCustomer(command);

      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal(validationFailures, badRequestResult.Value);
    }
    
    [Theory, AutoData]
    public async Task WHEN_POST_customer_with_valid_command_THEN_creates_new_customer_and_returns_NoContent(CreateCustomerCommand command) {
      var result = await _sut.CreateCustomer(command);

      Assert.IsType<NoContentResult>(result);
      A.CallTo(() => _createCustomerCommandHandler.Handle(command)).MustHaveHappened();
    }
  }
}