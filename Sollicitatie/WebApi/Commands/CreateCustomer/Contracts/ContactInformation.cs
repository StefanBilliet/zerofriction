namespace WebApi.Commands.CreateCustomer.Contracts {
  public class ContactInformation {
    public ContactInformationType Type { get; set; }
    public string Value { get; set; } = "";
  }
}