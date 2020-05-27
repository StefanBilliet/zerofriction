using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain.State;
using FluentValidation;
using Tests.Infrastructure;
using WebApi.Commands.CreateCustomer;
using WebApi.Commands.CreateCustomer.Contracts;
using Xunit;
using ContactInformation = WebApi.Commands.Shared.Contracts.ContactInformation;
using ContactInformationType = WebApi.Commands.Shared.Contracts.ContactInformationType;

namespace Tests.Commands.CreateCustomer {
  public class AddCustomerCommandHandlerTests {
    private readonly DummyCustomerRepository _customerRepository;
    private readonly AddCustomerCommandHandler _sut;
    private readonly Fixture _fixture;

    public AddCustomerCommandHandlerTests() {
      _customerRepository = new DummyCustomerRepository();
      _sut = new AddCustomerCommandHandler(new AddCustomerCommandValidator(), _customerRepository);
      _fixture = new Fixture();
    }

    [Fact]
    public async Task GIVEN_command_is_not_valid_WHEN_Handle_THEN_throws() {
      var command = _fixture
        .Build<AddCustomerCommand>()
        .With(_ => _.FirstName, default(string))
        .Create();

      await Assert.ThrowsAsync<ValidationException>(() => _sut.Handle(command));
    }

    [Fact]
    public async Task GIVEN_command_is_valid_WHEN_Handle_THEN_adds_new_customer() {
      var command = _fixture
        .Build<AddCustomerCommand>()
        .With(_ => _.ContactDetails, new[] {
          new ContactInformation {
            Type = ContactInformationType.Email,
            Value = "joske@gmail.com"
          }
        })
        .Create();

      await _sut.Handle(command);
      
      AssertEx.Equal(new CustomerState {
        Id = command.Id,
        FirstName = command.FirstName,
        SurName = command.SurName,
        Address = new Domain.State.Address {
          Street = command.Address.Street,
          NumberAndSuffix = command.Address.NumberAndSuffix,
          Area = command.Address.Area,
          AreaCode = command.Address.AreaCode
        },
        ContactDetails = command.ContactDetails.Select(_ => new Domain.State.ContactInformation {
          Type = (Domain.State.ContactInformationType) _.Type,
          Value = _.Value
        }).ToArray()
      }, _customerRepository.UpsertedCustomer);
    }
  }
}