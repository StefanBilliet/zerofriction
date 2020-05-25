using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Commands.CreateCustomer;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared.Validators;
using Xunit;

namespace WebApi.Tests.Commands.CreateCustomer {
  public class AddCustomerCommandValidatorTests {
    private readonly IValidator<AddCustomerCommand> _sut;

    public AddCustomerCommandValidatorTests() {
      _sut = new AddCustomerCommandValidator();
    }
    
    [Fact]
    public void Null_FirstName_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.FirstName, default(string));
    }
    
    [Fact]
    public void Empty_FirstName_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.FirstName, "");
    }
    
    [Fact]
    public void Non_empty_FirstName_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.FirstName, "Joske");
    }
    
    [Fact]
    public void Null_SurName_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.SurName, default(string));
    }
    
    [Fact]
    public void Empty_SurName_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.SurName, "");
    }
    
    [Fact]
    public void Non_empty_SurName_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.SurName, "Vermeulen");
    }

    [Fact]
    public void It_should_validate_the_address() {
      _sut.ShouldHaveChildValidator(_ => _.Address, typeof(CustomerAddressValidator));
    }
    
    [Fact]
    public void It_should_validate_the_contact_details() {
      _sut.ShouldHaveChildValidator(_ => _.ContactDetails, typeof(ContactInformationValidator));
    }
  }
}