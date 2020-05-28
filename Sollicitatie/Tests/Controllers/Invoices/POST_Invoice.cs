using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FakeItEasy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.InvoiceCustomer.Contracts;
using Xunit;

namespace Tests.Controllers.Invoices {
  public partial class InvoicesControllerTests {
    [Theory, AutoData]
    public async Task GIVEN_no_customer_with_id_WHEN_invoice_customer_THEN_returns_NotFound(InvoiceCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(command.CustomerId)).Returns(false);

      var result = await _sut.InvoiceCustomer(command);

      Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Theory, AutoData]
    public async Task WHEN_POST_invoice_with_invalid_command_THEN_returns_BadRequest(InvoiceCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(command.CustomerId)).Returns(true);
      var validationFailures = new []{new ValidationFailure(nameof(InvoiceCustomerCommand.Description), "required") };
      A.CallTo(() => _invoiceCustomerCommandHandler.Handle(command)).Throws(new ValidationException(validationFailures));
      
      var result = await _sut.InvoiceCustomer(command);

      var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
      Assert.Equal(validationFailures, badRequestResult.Value);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_customer_with_id_WHEN_invoice_customer_THEN_invoices_customer_and_returns_NoContent(InvoiceCustomerCommand command) {
      A.CallTo(() => _customerDataService.CustomerExists(command.CustomerId)).Returns(true);

      var result = await _sut.InvoiceCustomer(command);

      Assert.IsType<NoContentResult>(result);
    }
  }
}