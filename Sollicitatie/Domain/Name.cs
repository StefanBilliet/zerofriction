using System;
using System.Collections.Generic;
using Domain.Shared;

namespace Domain {
  public class Name : ValueObject<Name> {
    public string FirstName { get; }
    public string SurName { get; }

    public Name(string firstName, string surName) {
      FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
      SurName = surName ?? throw new ArgumentNullException(nameof(surName));
    }
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return FirstName;
      yield return SurName;
    }
  }
}