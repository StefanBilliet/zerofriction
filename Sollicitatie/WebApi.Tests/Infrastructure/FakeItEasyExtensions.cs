using FakeItEasy;
using KellermanSoftware.CompareNetObjects;

namespace WebApi.Tests.Infrastructure {
  public static class FakeItEasyExtensions {
    public static T MatchesObject<T>(this IArgumentConstraintManager<T> scope, object other) {
      var comparer = new CompareLogic();
      return scope.Matches(thisObject => comparer.Compare(thisObject, other).AreEqual);
    }
  }
}