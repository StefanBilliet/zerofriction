using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.CreateCustomer.Contracts;
using Xunit;

namespace Tests.Controllers.Customers {
  public partial class CustomersControllerTests {
    [Theory, AutoData]
    public async Task GIVEN_no_customer_with_id_WHEN_add_contact_details_THEN_returns_NotFound(Guid customerId, AddContactDetailsForCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(customerId)).Returns(false);

      var result = await _sut.AddContactDetailsForCustomer(customerId, command);

      Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_customer_with_id_GIVEN_invalid_command_WHEN_add_contact_details_THEN_returns_BadRequest(Guid customerId, AddContactDetailsForCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(customerId)).Returns(true);
      var validationFailures = new []{new ValidationFailure(nameof(AddContactDetailsForCustomerCommand.ContactInformation.Value), "required") };
      A.CallTo(() => _addContactDetailsForCustomerCommandHandler.Handle(command)).Throws(new ValidationException(validationFailures));

      var result = await _sut.AddContactDetailsForCustomer(customerId, command);

      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal(validationFailures, badRequestResult.Value);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_customer_with_id_GIVEN_valid_command_WHEN_add_contact_details_THEN_adds_contact_details_to_customer(Guid customerId, AddContactDetailsForCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(customerId)).Returns(true);

      var result = await _sut.AddContactDetailsForCustomer(customerId, command);

      Assert.IsType<NoContentResult>(result);
      A.CallTo(() => _addContactDetailsForCustomerCommandHandler.Handle(command)).MustHaveHappened();
    }
  }
}