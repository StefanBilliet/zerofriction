using Domain.Customers;

namespace WebApi.Commands.Shared.Contracts {
  public class ContactInformation {
    public ContactInformationType Type { get; set; }
    public string Value { get; set; } = "";
  }
}