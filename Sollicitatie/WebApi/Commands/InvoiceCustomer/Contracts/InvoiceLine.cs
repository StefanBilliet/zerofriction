namespace WebApi.Commands.InvoiceCustomer.Contracts {
  public class InvoiceLine {
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalAmount { get; set; }
  }
}