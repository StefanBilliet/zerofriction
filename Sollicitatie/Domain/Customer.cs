using System;
using System.Linq;
using Domain.Shared;
using Domain.State;

namespace Domain {
  public class Customer : AggregateRoot<CustomerState> {
    private readonly Name _name;
    private readonly Address _address;
    private readonly ContactDetails _contactDetails;

    public Customer(Guid id, Name name, Address address, ContactDetails contactDetails) {
      _name = name ?? throw new ArgumentNullException(nameof(name));
      _address = address ?? throw new ArgumentNullException(nameof(address));
      _contactDetails = contactDetails ?? throw new ArgumentNullException(nameof(contactDetails));

      Id = id;
    }

    public override CustomerState Deflate() {
      return new CustomerState {
        Id = Id,
        FirstName = _name.FirstName,
        SurName = _name.SurName,
        Address = new State.Address {
          Street = _address.Street,
          NumberAndSuffix = _address.NumberAndSuffix,
          Area = _address.Area,
          AreaCode = _address.AreaCode
        },
        ContactDetails = _contactDetails.ContactInformation.Select(_ => new Domain.State.ContactInformation {
          Type = (State.ContactInformationType) _.Type,
          Value = _.Value
        }).ToArray()
      };
    }
  }
}