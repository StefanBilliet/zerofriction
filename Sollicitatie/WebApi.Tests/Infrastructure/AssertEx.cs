using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace WebApi.Tests.Infrastructure {
  public static class AssertEx {
    public static void Equal<T>(T expected, T actual) {
      var comparer = new CompareLogic(new ComparisonConfig {
        MaxDifferences = 100
      });
      var comparisonResult = comparer.Compare(expected, actual);
      Assert.True(comparisonResult.AreEqual, comparisonResult.DifferencesString);
    }
  }
}