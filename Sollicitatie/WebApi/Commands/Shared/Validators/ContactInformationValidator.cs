using FluentValidation;
using WebApi.Commands.CreateCustomer.Contracts;

namespace WebApi.Commands.Shared.Validators {
  public class ContactInformationValidator : AbstractValidator<ContactInformation> {
  }
}