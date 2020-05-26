using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class ContactInformation {
    public ContactInformationType Type { get; set; }
    public string Value { get; set; } = "";
  }
}