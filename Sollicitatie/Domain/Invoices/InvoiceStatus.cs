namespace Domain.Invoices {
  public enum InvoiceStatus {
    Draft,
    Sent,
    PartiallyPaid,
    PaidInFull,
    Overdue,
    Canceled
  }
}