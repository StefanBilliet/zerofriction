using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Domain.Exceptions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using Xunit;

namespace Tests.Controllers.Invoices {
  public partial class InvoicesControllerTests {
    [Theory, AutoData]
    public async Task GIVEN_no_invoice_with_id_WHEN_change_invoice_status_THEN_returns_NotFound(ChangeInvoiceStatusCommand command) {
      A.CallTo(() => _invoiceDataService.InvoiceExists(command.Id)).Returns(false);

      var result = await _sut.ChangeInvoiceStatus(command.Id, command);

      Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_invoice_with_id_WHEN_trying_to_change_invoice_status_to_illegal_status_THEN_returns_BadRequest(ChangeInvoiceStatusCommand command) {
      A.CallTo(() => _invoiceDataService.InvoiceExists(command.Id)).Returns(true);
      A.CallTo(() => _changeInvoiceStatusCommandHandler.Handle(command)).Throws<IllegalInvoiceStatusTransitionException>();

      var result = await _sut.ChangeInvoiceStatus(command.Id, command);

      Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Theory, AutoData]
    public async Task GIVEN_invoice_with_id_WHEN_change_invoice_status_THEN_invoices_customer_and_returns_NoContent(ChangeInvoiceStatusCommand command) {
      A.CallTo(() => _invoiceDataService.InvoiceExists(command.Id)).Returns(true);

      var result = await _sut.ChangeInvoiceStatus(command.Id, command);

      Assert.IsType<NoContentResult>(result);
    }
  }
}