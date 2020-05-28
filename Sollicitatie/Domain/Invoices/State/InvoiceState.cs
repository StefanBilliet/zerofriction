using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Domain.Invoices.State {
  [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))] 
  public class InvoiceState {
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; } = "";
    public Guid CustomerId { get; set; }
    public InvoiceLine[] InvoiceLines { get; set; } = new InvoiceLine[0];
    public decimal TotalAmount { get; set; }
    public string TenantId { get; set; } = "";
  }
}