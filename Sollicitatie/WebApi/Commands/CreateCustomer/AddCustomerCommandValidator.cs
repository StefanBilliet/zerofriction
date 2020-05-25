using FluentValidation;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared.Validators;

namespace WebApi.Commands.CreateCustomer {
  public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand> {
    public AddCustomerCommandValidator() {
      RuleFor(_ => _.FirstName).NotEmpty();
      RuleFor(_ => _.SurName).NotEmpty();
      RuleFor(_ => _.Address).SetValidator(new CustomerAddressValidator());
      RuleForEach(_ => _.ContactDetails).SetValidator(new ContactInformationValidator());
    }
  }
}