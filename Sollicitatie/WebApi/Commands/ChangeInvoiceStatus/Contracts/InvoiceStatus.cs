namespace WebApi.Commands.ChangeInvoiceStatus.Contracts {
  public enum InvoiceStatus {
    Draft,
    Sent,
    PartiallyPaid,
    PaidInFull,
    Overdue,
    Canceled
  }
}