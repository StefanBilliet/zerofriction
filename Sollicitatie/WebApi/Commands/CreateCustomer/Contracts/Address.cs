namespace WebApi.Commands.CreateCustomer.Contracts {
  public class Address {
    public string Street { get; set; } = "";
    public string NumberAndSuffix { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public string Area { get; set; } = "";
  }
}