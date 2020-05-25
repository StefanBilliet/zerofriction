using System;
using System.Collections.Generic;

namespace WebApi.Commands.CreateCustomer {
  public class CreateCustomerCommand {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string SurName { get; set; }
    public Address Address { get; set; }
    public IReadOnlyCollection<ContactInformation> ContactDetails { get; set; }
  }
}