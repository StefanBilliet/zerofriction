using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.Customers.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class CustomerState {
    public Guid Id { get; set; }
    public string FirstName { get; set; } = "";
    public string SurName { get; set; } = "";
    public Address Address { get; set; } = new Address();
    public ContactInformation[] ContactDetails { get; set; } = new ContactInformation[0];
    public string TenantId { get; set; } = "";
  }
}