namespace WebApi.Commands.InvoiceCustomer.Contracts {
  public enum InvoiceStatus {
    Draft,
    Sent,
    PartiallyPaid,
    PaidInFull,
    Overdue,
    Canceled
  }
}