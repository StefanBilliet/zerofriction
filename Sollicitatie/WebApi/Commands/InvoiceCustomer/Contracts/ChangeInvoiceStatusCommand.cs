using System;

namespace WebApi.Commands.InvoiceCustomer.Contracts {
  public class ChangeInvoiceStatusCommand {
    public Guid Id { get; set; }
    public InvoiceStatus Status { get; set; }
  }
}