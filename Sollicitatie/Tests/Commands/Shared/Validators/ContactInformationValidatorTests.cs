using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared.Contracts;
using WebApi.Commands.Shared.Validators;
using Xunit;

namespace Tests.Commands.Shared.Validators {
  public class ContactInformationValidatorTests {
    private readonly IValidator<ContactInformation> _sut;

    public ContactInformationValidatorTests() {
      _sut = new ContactInformationValidator();
    }

    [Fact]
    public void WHEN_type_is_email_and_value_is_not_a_valid_email_THEN_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Value, new ContactInformation {Type = ContactInformationType.Email, Value = "not an email"});
    }
    
    [Fact]
    public void WHEN_type_is_email_and_value_is_a_valid_email_THEN_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Value, new ContactInformation {Type = ContactInformationType.Email, Value = "joske@gmaiL.com"});
    }
    
    [Fact]
    public void WHEN_type_is_cell_phone_and_value_is_not_a_valid_cell_phone_THEN_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Value, new ContactInformation {Type = ContactInformationType.CellPhone, Value = "not an phone number"});
    }
    
    [Fact]
    public void WHEN_type_is_cell_phone_and_value_is_a_valid_cell_phone_THEN_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Value, new ContactInformation {Type = ContactInformationType.CellPhone, Value = "0475360421"});
    }
  }
}