using System;
using System.Runtime.Serialization;
using Domain.Invoices;

namespace Domain.Exceptions {
  [Serializable]
  public class IllegalInvoiceStatusTransitionException : Exception {
    public IllegalInvoiceStatusTransitionException() {
    }

    public IllegalInvoiceStatusTransitionException(InvoiceStatus currentStatus, InvoiceStatus newStatus) : base($"An invoice cannot transition from ${currentStatus} to ${newStatus}") {
    }

    public IllegalInvoiceStatusTransitionException(string message) : base(message) {
    }

    public IllegalInvoiceStatusTransitionException(string message, Exception innerException) : base(message, innerException) {
    }

    protected IllegalInvoiceStatusTransitionException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
  }
}