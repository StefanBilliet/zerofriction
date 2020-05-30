namespace WebApi.Commands.Shared.Contracts {
  public class Address {
    public string Street { get; set; } = "";
    public string NumberAndSuffix { get; set; } = "";
    public string City { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public string Area { get; set; } = "";
  }
}