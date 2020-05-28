using System.Collections.Generic;
using Domain.Shared;

namespace Domain.Invoices {
  public class InvoiceLine : ValueObject<InvoiceLine> {
    public int Quantity { get; }
    public decimal PricePerUnit { get; }
    public decimal TotalAmount { get; }
    
    public InvoiceLine(int quantity, decimal pricePerUnit, decimal totalAmount) {
      Quantity = quantity;
      PricePerUnit = pricePerUnit;
      TotalAmount = totalAmount;
    }
    
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return Quantity;
      yield return PricePerUnit;
      yield return TotalAmount;
    }
  }
}