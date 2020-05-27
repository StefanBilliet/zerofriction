using FluentValidation;
using PhoneNumbers;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared.Contracts;

namespace WebApi.Commands.Shared.Validators {
  public class ContactInformationValidator : AbstractValidator<ContactInformation> {
    private static readonly PhoneNumberUtil PhoneNumberUtil = PhoneNumberUtil.GetInstance();
    public ContactInformationValidator() {
      RuleFor(_ => _.Value).EmailAddress().When(_ => _.Type == ContactInformationType.Email);
      RuleFor(_ => _.Value).Must(BeAPhoneNumber).When(_ => _.Type == ContactInformationType.CellPhone).WithMessage(contactInformation =>
        $"{contactInformation.Value} is not a valid phone number.");
    }

    private static bool BeAPhoneNumber(string value) {
      //To be comprehensive, this should probably be validated by a service like Twilio
      return PhoneNumberUtil.IsPossibleNumber(value, "BE");
    }
  }
}