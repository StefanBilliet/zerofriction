using System;
using System.Collections.Generic;

namespace WebApi.Commands.CreateCustomer.Contracts {
  public class AddCustomerCommand {
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string SurName { get; set; } = "";
    public Address Address { get; set; } = new Address();
    public IReadOnlyCollection<ContactInformation> ContactDetails { get; set; } = new ContactInformation[0];
  }
}