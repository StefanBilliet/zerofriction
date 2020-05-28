using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.Customers.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class Address {
    public string Street { get; set; } = "";
    public string NumberAndSuffix { get; set; } = "";
    public string City { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public string Area { get; set; } = "";
  }
}