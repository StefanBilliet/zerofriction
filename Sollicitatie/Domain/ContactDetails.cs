using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Shared;

namespace Domain {
  public class ContactDetails : ValueObject<ContactDetails> {
    private readonly List<ContactInformation> _contactInformation;
    public IReadOnlyCollection<ContactInformation> ContactInformation => _contactInformation;

    public ContactDetails(ContactInformation[] contactInformation) {
      _contactInformation = contactInformation.ToList() ?? throw new ArgumentNullException(nameof(contactInformation));
    }
    
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return ContactInformation;
    }

    public void AddContactInformation(ContactInformation contactInformation) {
      _contactInformation.Add(contactInformation);
    }
  }
}