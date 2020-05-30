using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Domain.Customers;
using FakeItEasy;
using FluentValidation;
using Tests.TestingUtilities;
using WebApi.Commands.AddContactDetailsForCustomer;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using Xunit;
using Address = Domain.Customers.Address;
using ContactInformation = WebApi.Commands.Shared.Contracts.ContactInformation;

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
        new Address("Korenmarkt", "1A", "Gent", "9000", "Oost-Vlaanderen"),
        new ContactDetails(new Domain.Customers.ContactInformation[0])
      );
      A.CallTo(() => _customerRepository.Get(command.Id)).Returns(customer);

      await _sut.Handle(command);

      A.CallTo(() => _customerRepository.Upsert(A<Customer>.That.Matches(_ =>
        _.Deflate().ContactDetails.DeepEquals(
          new[] {
            new Domain.Customers.State.ContactInformation {
              Type = command.ContactInformation.Type,
              Value = command.ContactInformation.Value
            }
          })))).MustHaveHappened();
    }
  }
}