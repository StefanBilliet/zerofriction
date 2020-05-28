using System;
using System.Threading.Tasks;
using Data.Repositories;
using Domain;
using Domain.Customers;
using FluentValidation;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi.Commands.AddContactDetailsForCustomer {
  public class AddContactDetailsForCustomerCommandHandler : ICommandHandler<AddContactDetailsForCustomerCommand> {
    private readonly IValidator<AddContactDetailsForCustomerCommand> _validator;
    private readonly ICustomerRepository _customerRepository;

    public AddContactDetailsForCustomerCommandHandler(IValidator<AddContactDetailsForCustomerCommand> validator, ICustomerRepository customerRepository) {
      _validator = validator ?? throw new ArgumentNullException(nameof(validator));
      _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task Handle(AddContactDetailsForCustomerCommand command) {
      await _validator.ValidateAndThrowAsync(command);

      var customer = await _customerRepository.Get(command.Id);
      customer.AddContactInformation(new ContactInformation((ContactInformationType) command.ContactInformation.Type, command.ContactInformation.Value));

      await _customerRepository.Upsert(customer);
    }
  }
}