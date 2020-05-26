using FakeItEasy;
using WebApi.Commands;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Controllers;

namespace Tests.Controllers.Customers {
  public partial class CustomersControllerTests {
    private readonly ICommandHandler<AddCustomerCommand> _addCustomerCommandHandler;
    private readonly CustomersController _sut;

    public CustomersControllerTests() {
      _addCustomerCommandHandler = A.Fake<ICommandHandler<AddCustomerCommand>>();
      _sut = new CustomersController(_addCustomerCommandHandler);
    }
  }
}