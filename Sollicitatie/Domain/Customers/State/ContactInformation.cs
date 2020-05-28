using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.Customers.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class ContactInformation {
    public ContactInformationType Type { get; set; }
    public string Value { get; set; } = "";
  }
}