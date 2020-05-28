using FluentValidation;
using WebApi.Commands.InvoiceCustomer.Contracts;

namespace WebApi.Commands.InvoiceCustomer {
  public class InvoiceCustomerCommandValidator : AbstractValidator<InvoiceCustomerCommand> {
    public InvoiceCustomerCommandValidator() {
      RuleFor(_ => _.Description).NotEmpty();
      //you could have the discussion: are the following two rules enforcing domain invariants, or just filtering out nonsensical data?
      RuleFor(_ => _.TotalAmount).GreaterThanOrEqualTo(0);
      RuleFor(_ => _.InvoiceLines).Must(_ => _.Count >= 1)
        .WithMessage("An invoice must have at least one invoice line");
      RuleForEach(_ => _.InvoiceLines).SetValidator(new InvoiceLineValidator());
    }
  }
}