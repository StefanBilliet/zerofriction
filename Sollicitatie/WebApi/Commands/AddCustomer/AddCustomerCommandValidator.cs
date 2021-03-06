using FluentValidation;
using WebApi.Commands.AddCustomer.Contracts;
using WebApi.Commands.Shared.Validators;

namespace WebApi.Commands.AddCustomer {
  public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand> {
    public AddCustomerCommandValidator() {
      RuleFor(_ => _.FirstName).NotEmpty();
      RuleFor(_ => _.SurName).NotEmpty();
      RuleFor(_ => _.Address).SetValidator(new AddressValidator());
      RuleForEach(_ => _.ContactDetails).SetValidator(new ContactInformationValidator());
    }
  }
}