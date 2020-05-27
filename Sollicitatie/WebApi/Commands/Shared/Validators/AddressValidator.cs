using FluentValidation;
using WebApi.Commands.Shared.Contracts;

namespace WebApi.Commands.Shared.Validators {
  public class AddressValidator : AbstractValidator<Address> {
    public AddressValidator() {
      RuleFor(_ => _.Street).NotEmpty();
      RuleFor(_ => _.NumberAndSuffix).NotEmpty();
      RuleFor(_ => _.Area).NotEmpty();
      RuleFor(_ => _.AreaCode).NotEmpty();
    }
  }
}