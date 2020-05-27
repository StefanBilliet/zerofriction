using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Domain;
using Domain.State;
using FakeItEasy;
using FluentValidation;
using Tests.Infrastructure;
using WebApi.Commands.AddContactDetailsForCustomer;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using Xunit;
using Address = Domain.Address;
using ContactInformation = WebApi.Commands.Shared.Contracts.ContactInformation;
using ContactInformationType = WebApi.Commands.Shared.Contracts.ContactInformationType;

namespace Tests.Commands.AddContactDetailsForCustomer {
  public class AddContactDetailsForCustomerCommandHandlerTests {
    private readonly ICustomerRepository _customerRepository;
    private readonly AddContactDetailsForCustomerCommandHandler _sut;
    private readonly Fixture _fixture;

    public AddContactDetailsForCustomerCommandHandlerTests() {
      _customerRepository = A.Fake<ICustomerRepository>();
      _sut = new AddContactDetailsForCustomerCommandHandler(new AddContactDetailsForCustomerCommandValidator(),
        _customerRepository);
      _fixture = new Fixture();
    }

    [Fact]
    public async Task GIVEN_command_is_not_valid_WHEN_Handle_THEN_throws() {
      var command = _fixture
        .Build<AddContactDetailsForCustomerCommand>()
        .With(_ => _.ContactInformation, new ContactInformation {
          Type = ContactInformationType.Email,
          Value = "not an email"
        })
        .Create();

      await Assert.ThrowsAsync<ValidationException>(() => _sut.Handle(command));
    }

    [Fact]
    public async Task GIVEN_command_is_valid_WHEN_Handle_THEN_adds_new_customer() {
      var command = _fixture
        .Build<AddContactDetailsForCustomerCommand>()
        .With(_ => _.ContactInformation, new ContactInformation {
          Type = ContactInformationType.Email,
          Value = "joske@gmail.com"
        })
        .Create();
      var customer = new Customer(
        command.Id,
        new Name("Joske", "Vermeulen"),
        new Address("Korenmarkt", "1A", "9000", "Oost-Vlaanderen"),
        new ContactDetails(new Domain.ContactInformation[0])
      );
      A.CallTo(() => _customerRepository.Get(command.Id)).Returns(customer);
      Customer? changedCustomer = null;
      A.CallTo(() => _customerRepository.Upsert(A<Customer>._)).Invokes(call => changedCustomer = (Customer) call.Arguments[0]!);

      await _sut.Handle(command);
      
      AssertEx.Equal(new CustomerState {
        Id = command.Id,
        FirstName = "Joske",
        SurName = "Vermeulen",
        Address = new Domain.State.Address {
          Street = "Korenmarkt",
          NumberAndSuffix = "1A",
          Area = "Oost-Vlaanderen",
          AreaCode = "9000"
        },
        ContactDetails = new []{
          new Domain.State.ContactInformation {
          Type = (Domain.State.ContactInformationType) command.ContactInformation.Type,
          Value = command.ContactInformation.Value
        }}
      }, changedCustomer?.Deflate());
    }
  }
}