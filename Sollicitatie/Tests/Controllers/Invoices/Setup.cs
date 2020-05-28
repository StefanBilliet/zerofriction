using Data.DataServices;
using FakeItEasy;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;
using WebApi.Controllers;

namespace Tests.Controllers.Invoices {
  public partial class InvoicesControllerTests {
    private readonly ICustomerDataService _customerDataService;
    private readonly ICommandHandler<InvoiceCustomerCommand> _invoiceCustomerCommandHandler;
    private readonly InvoicesController _sut;
    
    public InvoicesControllerTests() {
      _customerDataService = A.Fake<ICustomerDataService>();
      _invoiceCustomerCommandHandler = A.Fake<ICommandHandler<InvoiceCustomerCommand>>();
      _sut = new InvoicesController(_customerDataService, _invoiceCustomerCommandHandler);
    }
  }
}