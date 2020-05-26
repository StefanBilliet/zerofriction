using System;
using System.Collections.Generic;
using Domain.Shared;

namespace Domain {
  public class ContactDetails : ValueObject<ContactDetails> {
    public IReadOnlyCollection<ContactInformation> ContactInformation { get; }

    public ContactDetails(ContactInformation[] contactInformation) {
      ContactInformation = contactInformation ?? throw new ArgumentNullException(nameof(contactInformation));
    }
    
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return ContactInformation;
    }
  }
}