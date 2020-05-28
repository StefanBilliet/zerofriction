using FluentValidation.TestHelper;
using WebApi.Commands.InvoiceCustomer;
using Xunit;

namespace Tests.Commands.InvoiceCustomer {
  public class InvoiceLineValidatorTests {
    private readonly InvoiceLineValidator _sut;

    public InvoiceLineValidatorTests() {
      _sut = new InvoiceLineValidator();
    }
    
    [Fact]
    public void Negative_quantity_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Quantity, -1);
    }

    [Fact]
    public void Zero_quantity_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Quantity, 0);
    }

    [Fact]
    public void Higher_than_zero_quantity_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Quantity, 5000);
    }
    
    [Fact]
    public void Negative_price_per_unit_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.PricePerUnit, -1);
    }

    [Fact]
    public void Zero_price_per_unit_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Quantity, 0);
    }

    [Fact]
    public void Higher_than_zero_price_per_unit_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Quantity, 5000);
    }

    [Fact]
    public void Negative_total_amount_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.TotalAmount, -1);
    }

    [Fact]
    public void Zero_total_amount_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.TotalAmount, 0);
    }

    [Fact]
    public void Higher_than_zero_total_amount_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.TotalAmount, 5000);
    }
  }
}