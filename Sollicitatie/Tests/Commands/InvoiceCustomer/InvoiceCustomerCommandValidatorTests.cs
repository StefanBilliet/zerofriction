using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using WebApi.Commands.InvoiceCustomer;
using WebApi.Commands.InvoiceCustomer.Contracts;
using Xunit;

namespace Tests.Commands.InvoiceCustomer {
  public class InvoiceCustomerValidatorTests {
    private readonly InvoiceCustomerCommandValidator _sut;

    public InvoiceCustomerValidatorTests() {
      _sut = new InvoiceCustomerCommandValidator();
    }

    [Fact]
    public void Null_Description_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Description, default(string));
    }

    [Fact]
    public void Empty_Description_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.Description, "");
    }

    [Fact]
    public void Non_empty_Description_passes() {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.Description, "Joske");
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

    [Fact]
    public void No_invoice_lines_fails() {
      _sut.ShouldHaveValidationErrorFor(_ => _.InvoiceLines, new InvoiceLine[0]);
    }

    [Theory, AutoData]
    public void One_invoice_line_passes(InvoiceLine invoiceLine) {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.InvoiceLines, new[] {invoiceLine});
    }
    
    [Theory, AutoData]
    public void Multiple_invoice_lines_passes(InvoiceLine[] invoiceLines) {
      _sut.ShouldNotHaveValidationErrorFor(_ => _.InvoiceLines, invoiceLines);
    }

    [Fact]
    public void InvoiceLines_should_have_child_validator() {
      _sut.ShouldHaveChildValidator(_ => _.InvoiceLines, typeof(InvoiceLineValidator));
    }
  }
}