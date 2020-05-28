using System;
using System.Collections.Generic;

namespace WebApi.Commands.InvoiceCustomer.Contracts {
  public class InvoiceCustomerCommand {
    public DateTimeOffset Date { get; set; }
    public string Description { get; set; }
    public Guid CustomerId { get; set; }
    public IReadOnlyCollection<InvoiceLine> InvoiceLines { get; set; }
    public decimal TotalAmount { get; set; }
  }
}