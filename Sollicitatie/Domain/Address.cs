using System;
using System.Collections.Generic;
using Domain.Shared;

namespace Domain {
  public class Address : ValueObject<Address> {
    public string Street { get;  }
    public string NumberAndSuffix { get; }
    public string AreaCode { get; }
    public string Area { get; }

    public Address(string street, string numberAndSuffix, string areaCode, string area) {
      Street = street ?? throw new ArgumentNullException(nameof(street));
      NumberAndSuffix = numberAndSuffix ?? throw new ArgumentNullException(nameof(numberAndSuffix));
      AreaCode = areaCode ?? throw new ArgumentNullException(nameof(areaCode));
      Area = area ?? throw new ArgumentNullException(nameof(area));
    }
    protected override IEnumerable<object> GetEqualityComponents() {
      yield return Street;
      yield return NumberAndSuffix;
      yield return AreaCode;
      yield return Area;
    }
  }
}