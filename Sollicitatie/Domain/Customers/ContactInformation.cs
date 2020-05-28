using System;
using System.Collections.Generic;
using Domain.Shared;

namespace Domain.Customers {
  public class ContactInformation : ValueObject<ContactInformation> {
    public ContactInformationType Type { get; }
    public string Value { get; }

    public ContactInformation(ContactInformationType type, string value) {
      Type = type;
      Value = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return Type;
      yield return Value;
    }
  }
}