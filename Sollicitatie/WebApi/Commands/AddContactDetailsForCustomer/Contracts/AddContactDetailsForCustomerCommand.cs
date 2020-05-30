using System;
using ContactInformation = WebApi.Commands.Shared.Contracts.ContactInformation;

namespace WebApi.Commands.AddContactDetailsForCustomer.Contracts {
  public class AddContactDetailsForCustomerCommand {
    public Guid Id { get; set; }
    public ContactInformation ContactInformation { get; set; } = new ContactInformation();
  }
}