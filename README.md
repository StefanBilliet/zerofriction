# zerofriction

This has only been tested with the official CosmosDB emulator.

First run the Database project, which will set up a database on the local emulator; then you can start the WebApi project.  
The initial data partitioning strategy is partitioning by tenant, so that a tenant's data will all reside in the same partition, avoiding costly cross partition requests. The partition key is /tenantId.

The routes are:
* POST /api/customers
  *     public class AddCustomerCommand {
          public Guid Id { get; set; }
          public string FirstName { get; set; } = "";
          public string SurName { get; set; } = "";
          public Address Address { get; set; } = new Address();
          public IReadOnlyCollection<ContactInformation> ContactDetails { get; set; } = new ContactInformation[0];
        }
        public class Address {
          public string Street { get; set; } = "";
          public string NumberAndSuffix { get; set; } = "";
          public string City { get; set; } = "";
          public string AreaCode { get; set; } = "";
          public string Area { get; set; } = "";
        }
        public class ContactInformation {
          public ContactInformationType Type { get; set; }
          public string Value { get; set; } = "";
        }
        public enum ContactInformationType {
          Email,
          CellPhone
        }
* POST /api/customers/{id}/contactdetails
  *     public class AddContactDetailsForCustomerCommand {
          public Guid Id { get; set; }
          public ContactInformation ContactInformation { get; set; } = new ContactInformation();
        }
        public class ContactInformation {
          public ContactInformationType Type { get; set; }
          public string Value { get; set; } = "";
        }
        public enum ContactInformationType {
          Email,
          CellPhone
        }
* POST /api/invoices
  *     public class InvoiceCustomerCommand {
          public Guid Id { get; set; }
          public DateTimeOffset Date { get; set; }
          public string Description { get; set; } = "";
          public Guid CustomerId { get; set; }
          public IReadOnlyCollection<InvoiceLine> InvoiceLines { get; set; } = new InvoiceLine[0];
          public decimal TotalAmount { get; set; }
        }
        public class InvoiceLine {
          public int Quantity { get; set; }
          public decimal PricePerUnit { get; set; }
          public decimal TotalAmount { get; set; }
        }
* PUT /api/invoices/{id}/status
  *     public class ChangeInvoiceStatusCommand {
          public Guid Id { get; set; }
          public InvoiceStatus Status { get; set; }
        }
