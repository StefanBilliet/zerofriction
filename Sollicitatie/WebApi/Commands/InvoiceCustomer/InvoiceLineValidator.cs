using FluentValidation;
using WebApi.Commands.InvoiceCustomer.Contracts;

namespace WebApi.Commands.InvoiceCustomer {
  public class InvoiceLineValidator : AbstractValidator<InvoiceLine> {
    public InvoiceLineValidator() {
      RuleFor(_ => _.Quantity).GreaterThanOrEqualTo(1);
      RuleFor(_ => _.TotalAmount).GreaterThanOrEqualTo(0);
      RuleFor(_ => _.PricePerUnit).GreaterThanOrEqualTo(0);
    }
  }
}