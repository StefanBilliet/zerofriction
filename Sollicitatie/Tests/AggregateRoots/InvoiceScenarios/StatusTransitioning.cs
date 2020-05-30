using System;
using System.Collections.Generic;
using Domain.Exceptions;
using Domain.Invoices;
using Xunit;

namespace Tests.AggregateRoots.InvoiceScenarios {
  public class StatusTransitioning {
    public class FromDraft {
      [Theory]
      [MemberData(nameof(ValidTransitionsFromDraft))]
      public void GIVEN_draft_invoice_WHEN_transitioning_to_valid_status_THEN_changes_status_of_invoice(Action<Invoice> statusTransition, InvoiceStatus expectedStatus) {
        var invoice = Invoice(InvoiceStatus.Draft);

        statusTransition(invoice);
      
        Assert.Equal(expectedStatus, invoice.Deflate().Status);
      }

      public static IEnumerable<object[]> ValidTransitionsFromDraft() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsSent()),
          InvoiceStatus.Sent
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPartiallyPaid()),
          InvoiceStatus.PartiallyPaid
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPaidInFull()),
          InvoiceStatus.PaidInFull
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.Cancel()),
          InvoiceStatus.Canceled
        };
      }
    
      [Theory]
      [MemberData(nameof(InvalidTransitionsFromDraft))]
      public void GIVEN_draft_invoice_WHEN_trying_to_transition_to_invalid_status_THEN_throws(Action<Invoice> statusTransition) {
        var invoice = Invoice(InvoiceStatus.Draft);

        Assert.Throws<IllegalInvoiceStatusTransitionException>(() => statusTransition(invoice));
        Assert.Equal(InvoiceStatus.Draft, invoice.Deflate().Status);
      }
    
      public static IEnumerable<object[]> InvalidTransitionsFromDraft() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsOverdue())
        };
      }
    }
    
    public class FromSent {
      [Theory]
      [MemberData(nameof(ValidTransitionsFromSent))]
      public void GIVEN_sent_invoice_WHEN_transitioning_to_valid_status_THEN_changes_status_of_invoice(Action<Invoice> statusTransition, InvoiceStatus expectedStatus) {
        var invoice = Invoice(InvoiceStatus.Sent);

        statusTransition(invoice);
      
        Assert.Equal(expectedStatus, invoice.Deflate().Status);
      }

      public static IEnumerable<object[]> ValidTransitionsFromSent() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPartiallyPaid()),
          InvoiceStatus.PartiallyPaid
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPaidInFull()),
          InvoiceStatus.PaidInFull
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsOverdue()),
          InvoiceStatus.Overdue
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.Cancel()),
          InvoiceStatus.Canceled
        };
      }
    }
    
    public class FromPartiallyPaid {
      [Theory]
      [MemberData(nameof(ValidTransitionsFromPartiallyPaid))]
      public void GIVEN_partially_paid_invoice_WHEN_transitioning_to_valid_status_THEN_changes_status_of_invoice(Action<Invoice> statusTransition, InvoiceStatus expectedStatus) {
        var invoice = Invoice(InvoiceStatus.PartiallyPaid);

        statusTransition(invoice);
      
        Assert.Equal(expectedStatus, invoice.Deflate().Status);
      }

      public static IEnumerable<object[]> ValidTransitionsFromPartiallyPaid() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPaidInFull()),
          InvoiceStatus.PaidInFull
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsOverdue()),
          InvoiceStatus.Overdue
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.Cancel()),
          InvoiceStatus.Canceled
        };
      }
    
      [Theory]
      [MemberData(nameof(InvalidTransitionsFromPartiallyPaid))]
      public void GIVEN_partially_paid_invoice_WHEN_trying_to_transition_to_invalid_status_THEN_throws(Action<Invoice> statusTransition) {
        var invoice = Invoice(InvoiceStatus.PartiallyPaid);

        Assert.Throws<IllegalInvoiceStatusTransitionException>(() => statusTransition(invoice));
        Assert.Equal(InvoiceStatus.PartiallyPaid, invoice.Deflate().Status);
      }
    
      public static IEnumerable<object[]> InvalidTransitionsFromPartiallyPaid() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsSent())
        };
      }
    }
    
    public class FromPaidInFull {
      [Theory]
      [MemberData(nameof(ValidTransitionsFromPaidInFull))]
      public void GIVEN_fully_paid_invoice_WHEN_transitioning_to_valid_status_THEN_changes_status_of_invoice(Action<Invoice> statusTransition, InvoiceStatus expectedStatus) {
        var invoice = Invoice(InvoiceStatus.PaidInFull);

        statusTransition(invoice);
      
        Assert.Equal(expectedStatus, invoice.Deflate().Status);
      }

      public static IEnumerable<object[]> ValidTransitionsFromPaidInFull() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.Cancel()),
          InvoiceStatus.Canceled
        };
      }
    
      [Theory]
      [MemberData(nameof(InvalidTransitionsFromPaidInFull))]
      public void GIVEN_fully_paid_invoice_WHEN_trying_to_transition_to_invalid_status_THEN_throws(Action<Invoice> statusTransition) {
        var invoice = Invoice(InvoiceStatus.PaidInFull);

        Assert.Throws<IllegalInvoiceStatusTransitionException>(() => statusTransition(invoice));
        Assert.Equal(InvoiceStatus.PaidInFull, invoice.Deflate().Status);
      }
    
      public static IEnumerable<object[]> InvalidTransitionsFromPaidInFull() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsSent())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPartiallyPaid())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsOverdue())
        };
      }
    }
    
    public class FromOverdue {
      [Theory]
      [MemberData(nameof(ValidTransitionsFromOverdue))]
      public void GIVEN_overdue_invoice_WHEN_transitioning_to_valid_status_THEN_changes_status_of_invoice(Action<Invoice> statusTransition, InvoiceStatus expectedStatus) {
        var invoice = Invoice(InvoiceStatus.Overdue);

        statusTransition(invoice);
      
        Assert.Equal(expectedStatus, invoice.Deflate().Status);
      }

      public static IEnumerable<object[]> ValidTransitionsFromOverdue() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPaidInFull()),
          InvoiceStatus.PaidInFull
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.Cancel()),
          InvoiceStatus.Canceled
        };
      }
    
      [Theory]
      [MemberData(nameof(InvalidTransitionsFromOverdue))]
      public void GIVEN_overdue_invoice_WHEN_trying_to_transition_to_invalid_status_THEN_throws(Action<Invoice> statusTransition) {
        var invoice = Invoice(InvoiceStatus.Overdue);

        Assert.Throws<IllegalInvoiceStatusTransitionException>(() => statusTransition(invoice));
        Assert.Equal(InvoiceStatus.Overdue, invoice.Deflate().Status);
      }
    
      public static IEnumerable<object[]> InvalidTransitionsFromOverdue() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsSent())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPartiallyPaid())
        };
      }
    }
    
    public class FromCancelled {
      [Theory]
      [MemberData(nameof(InvalidTransitionsFromCancelled))]
      public void GIVEN_cancelled_invoice_WHEN_trying_to_transition_another_status_THEN_throws(Action<Invoice> statusTransition) {
        var invoice = Invoice(InvoiceStatus.Canceled);

        Assert.Throws<IllegalInvoiceStatusTransitionException>(() => statusTransition(invoice));
        Assert.Equal(InvoiceStatus.Canceled, invoice.Deflate().Status);
      }
    
      public static IEnumerable<object[]> InvalidTransitionsFromCancelled() {
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsSent())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPartiallyPaid())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsPaidInFull())
        };
        yield return new object[] {
          new Action<Invoice>(invoice => invoice.MarkAsOverdue())
        };
      }
    }

    private static Invoice Invoice(InvoiceStatus status) {
      return new Invoice(
        new Guid("6109AA78-2FE6-4519-ABBA-D0BAF6100DCE"),
        "Periodic invoice",
        new DateTimeOffset(new DateTime(2020, 01, 01)),
        new Guid("86D8629E-3E1E-4E35-9B1F-A1E591376CB6"),
        50,
        new[] {
          new InvoiceLine(1, 50, 50),
        },
        status
      );
    }
  }
}