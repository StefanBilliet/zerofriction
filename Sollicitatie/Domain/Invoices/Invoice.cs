using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Invoices.State;
using Domain.Shared;

namespace Domain.Invoices {
  public class Invoice : AggregateRoot<InvoiceState> {
    private readonly string _description;
    private readonly DateTimeOffset _date;
    private readonly Guid _customerId;
    private readonly decimal _totalAmount;
    private readonly List<InvoiceLine> _invoiceLines;
    private InvoiceStatus _status;

    public Invoice(Guid id, string description, DateTimeOffset date, Guid customerId, decimal totalAmount, InvoiceLine[] invoiceLines, InvoiceStatus status) {
      _description = description ?? throw new ArgumentNullException(nameof(description));
      _date = date;
      _customerId = customerId;
      _totalAmount = totalAmount;
      _status = status;
      _invoiceLines = invoiceLines.ToList() ?? throw new ArgumentNullException(nameof(invoiceLines));
      Id = id;
    }

    public override InvoiceState Deflate() {
      return new InvoiceState {
        Id = Id,
        Date = _date,
        Description = _description,
        CustomerId = _customerId,
        TotalAmount = _totalAmount,
        InvoiceLines = _invoiceLines.Select(_ => new Domain.Invoices.State.InvoiceLine {
          Quantity = _.Quantity,
          TotalAmount = _.TotalAmount,
          PricePerUnit = _.PricePerUnit
        }).ToArray()
      };
    }

    public static Invoice Draft(Guid id, string description, DateTimeOffset date, Guid customerId, decimal totalAmount, InvoiceLine[] invoiceLines) {
      return new Invoice(id, description, date, customerId, totalAmount, invoiceLines, InvoiceStatus.Draft);
    }
  }
}