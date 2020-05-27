using FluentValidation;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.Shared.Validators;

namespace WebApi.Commands.AddContactDetailsForCustomer {
  public class AddContactDetailsForCustomerCommandValidator : AbstractValidator<AddContactDetailsForCustomerCommand> {
    public AddContactDetailsForCustomerCommandValidator() {
      RuleFor(_ => _.ContactInformation).SetValidator(new ContactInformationValidator());
    }
  }
}