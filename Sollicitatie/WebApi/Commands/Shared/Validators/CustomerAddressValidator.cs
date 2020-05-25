using FluentValidation;
using WebApi.Commands.CreateCustomer.Contracts;

namespace WebApi.Commands.Shared.Validators {
  public class CustomerAddressValidator : AbstractValidator<Address> {
  }
}