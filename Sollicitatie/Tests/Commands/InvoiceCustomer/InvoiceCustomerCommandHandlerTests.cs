using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Domain.Invoices;
using Domain.Invoices.State;
using FakeItEasy;
using FluentValidation;
using Tests.TestingUtilities;
using WebApi.Commands.InvoiceCustomer;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;
using Xunit;
using InvoiceLine = WebApi.Commands.InvoiceCustomer.Contracts.InvoiceLine;

namespace Tests.Commands.InvoiceCustomer {
  public class InvoiceCustomerCommandHandlerTests {
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICommandHandler<InvoiceCustomerCommand> _sut;
    private readonly Fixture _fixture;

    public InvoiceCustomerCommandHandlerTests() {
      _invoiceRepository = A.Fake<IInvoiceRepository>();
      _sut = new InvoiceCustomerCommandHandler(new InvoiceCustomerCommandValidator(), _invoiceRepository);
      _fixture = new Fixture();
    }

    [Fact]
    public async Task GIVEN_invalid_command_WHEN_Handle_THEN_throws() {
      var command = _fixture
        .Build<InvoiceCustomerCommand>()
        .With(_ => _.Description, default(string))
        .Create();

      await Assert.ThrowsAsync<ValidationException>(() => _sut.Handle(command));
    }

    [Fact]
    public async Task GIVEN_valid_command_WHEN_Handle_THEN_creates_a_draft_invoice_for_the_customer() {
      var command = _fixture
        .Build<InvoiceCustomerCommand>()
        .With(_ => _.InvoiceLines, new [] {
          new InvoiceLine {
            Quantity = 5,
            PricePerUnit = 2,
            TotalAmount = 10
          }
        })
        .Create();

      await _sut.Handle(command);

      var state = new InvoiceState {
        Id = command.Id,
        Date = command.Date,
        Description = command.Description,
        CustomerId = command.CustomerId,
        TotalAmount = command.TotalAmount,
        InvoiceLines = command.InvoiceLines.Select(_ => new Domain.Invoices.State.InvoiceLine {
          Quantity = _.Quantity,
          TotalAmount = _.TotalAmount,
          PricePerUnit = _.PricePerUnit
        }).ToArray(),
        Status = Domain.Invoices.State.InvoiceStatus.Draft
      };
      A.CallTo(() => _invoiceRepository.Upsert(A<Invoice>.That.HasState(state))).MustHaveHappened();
    }
  }
}