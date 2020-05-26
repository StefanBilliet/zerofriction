using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Commands.CreateCustomer.Contracts;
using WebApi.Commands.Shared.Validators;
using Xunit;

namespace Tests.Commands.Shared.Validators {
  public class AddressValidatorTests {
    private readonly IValidator<Address> _sut;

    public AddressValidatorTests() {
      _sut = new AddressValidator();
    }

    [Fact]
    public void Null_street_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Street, default(string));
    }
    
    [Fact]
    public void Empty_street_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Street, "");
    }
    
    [Fact]
    public void Non_empty_street_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Street, "Korenmarkt");
    }
    
    [Fact]
    public void Null_number_and_suffix_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.NumberAndSuffix, default(string));
    }
    
    [Fact]
    public void Empty_number_and_suffix_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.NumberAndSuffix, "");
    }
    
    [Fact]
    public void Non_empty_number_and_suffix_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.NumberAndSuffix, "1 Bus 9");
    }
    
    [Fact]
    public void Non_empty_number_without_suffix_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.NumberAndSuffix, "1");
    }
    
    [Fact]
    public void Null_AreaCode_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.AreaCode, default(string));
    }
    
    [Fact]
    public void Empty_AreaCode_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.AreaCode, "");
    }
    
    [Fact]
    public void Non_empty_AreaCode_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.AreaCode, "9000");
    }
    
    [Fact]
    public void Null_Area_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Area, default(string));
    }
    
    [Fact]
    public void Empty_Area_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Area, "");
    }
    
    [Fact]
    public void Non_empty_Area_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Area, "Oost-Vlaanderen");
    }
  }
}