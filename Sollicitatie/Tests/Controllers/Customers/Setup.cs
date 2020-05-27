using Data.DataServices;
using FakeItEasy;
using WebApi.Commands;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared;
using WebApi.Controllers;

namespace Tests.Controllers.Customers {
  public partial class CustomersControllerTests {
    private readonly ICustomerDataService _customerDataService;
    private readonly ICommandHandler<AddCustomerCommand> _addCustomerCommandHandler;
    private readonly ICommandHandler<AddContactDetailsForCustomerCommand> _addContactDetailsForCustomerCommandHandler;
    private readonly CustomersController _sut;

    public CustomersControllerTests() {
      _customerDataService = A.Fake<ICustomerDataService>();
      _addCustomerCommandHandler = A.Fake<ICommandHandler<AddCustomerCommand>>();
      _addContactDetailsForCustomerCommandHandler = A.Fake<ICommandHandler<AddContactDetailsForCustomerCommand>>();
      _sut = new CustomersController(_addCustomerCommandHandler, _addContactDetailsForCustomerCommandHandler, _customerDataService);
    }
  }
}