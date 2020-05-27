using System;
using System.Threading.Tasks;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Commands.AddContactDetailsForCustomer {
  public class AddContactDetailsForCustomerCommandHandler : ICommandHandler<AddContactDetailsForCustomerCommand> {
    public Task Handle(AddContactDetailsForCustomerCommand command) {
      throw new NotImplementedException();
    }
  }
}