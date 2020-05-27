using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Domain;
using FluentValidation;
using WebApi.Commands.AddCustomer.Contracts;
using WebApi.Commands.Shared;
using Address = Domain.Address;
using ContactInformation = Domain.ContactInformation;
using ContactInformationType = Domain.ContactInformationType;

namespace WebApi.Commands.AddCustomer {
  public class AddCustomerCommandHandler : ICommandHandler<AddCustomerCommand> {
    private readonly IValidator<AddCustomerCommand> _validator;
    private readonly ICustomerRepository _customerRepository;

    public AddCustomerCommandHandler(IValidator<AddCustomerCommand> validator, ICustomerRepository customerRepository) {
      _validator = validator ?? throw new ArgumentNullException(nameof(validator));
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task Handle(AddCustomerCommand command) {
      await _validator.ValidateAndThrowAsync(command);

      var customer = new Customer(
        command.Id, 
        new Name(command.FirstName, command.SurName), 
        new Address(command.Address.Street, command.Address.NumberAndSuffix, command.Address.AreaCode, command.Address.Area), 
        new ContactDetails(command.ContactDetails.Select(_ => new ContactInformation((ContactInformationType) _.Type, _.Value)).ToArray()));

      await _customerRepository.Upsert(customer);
    }
  }
}