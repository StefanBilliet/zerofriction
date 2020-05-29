namespace Domain.Invoices.State {
  public enum InvoiceStatus {
    Draft,
    Sent,
    PartiallyPaid,
    PaidInFull,
    Overdue,
    Canceled
  }
}