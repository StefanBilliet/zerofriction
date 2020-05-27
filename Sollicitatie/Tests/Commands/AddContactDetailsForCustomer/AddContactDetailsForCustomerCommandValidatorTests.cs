using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Commands.AddContactDetailsForCustomer;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.Shared.Validators;
using Xunit;

namespace Tests.Commands.AddContactDetailsForCustomer {
  public class AddContactDetailsForCustomerCommandValidatorTests {
    private readonly IValidator<AddContactDetailsForCustomerCommand> _sut;

    public AddContactDetailsForCustomerCommandValidatorTests() {
      _sut = new AddContactDetailsForCustomerCommandValidator();
    }
    
    [Fact]
    public void It_should_validate_the_contact_details() {
      _sut.ShouldHaveChildValidator(_ => _.ContactInformation, typeof(ContactInformationValidator));
    }
  }
}