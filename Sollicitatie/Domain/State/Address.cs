using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class Address {
    public string Street { get; set; } = "";
    public string NumberAndSuffix { get; set; } = "";
    public string AreaCode { get; set; } = "";
    public string Area { get; set; } = "";
  }
}