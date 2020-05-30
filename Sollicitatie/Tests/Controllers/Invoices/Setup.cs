using Data.DataServices;
using FakeItEasy;
using WebApi.Commands.ChangeInvoiceStatus.Contracts;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;
using WebApi.Controllers;

namespace Tests.Controllers.Invoices {
  public partial class InvoicesControllerTests {
    private readonly ICustomerDataService _customerDataService;
    private readonly IInvoiceDataService _invoiceDataService;
    private readonly ICommandHandler<InvoiceCustomerCommand> _invoiceCustomerCommandHandler;
    private readonly ICommandHandler<ChangeInvoiceStatusCommand> _changeInvoiceStatusCommandHandler;
    private readonly InvoicesController _sut;
    
    public InvoicesControllerTests() {
      _customerDataService = A.Fake<ICustomerDataService>();
      _invoiceDataService = A.Fake<IInvoiceDataService>();
      _invoiceCustomerCommandHandler = A.Fake<ICommandHandler<InvoiceCustomerCommand>>();
      _changeInvoiceStatusCommandHandler = A.Fake<ICommandHandler<ChangeInvoiceStatusCommand>>();
      _sut = new InvoicesController(_customerDataService, _invoiceDataService, _invoiceCustomerCommandHandler, _changeInvoiceStatusCommandHandler);
    }
  }
}