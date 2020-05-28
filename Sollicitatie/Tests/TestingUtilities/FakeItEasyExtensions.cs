using Domain.Shared;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;

namespace Tests.TestingUtilities {
  public static class FakeItEasyExtensions {
    public static T MatchesObject<T>(this IArgumentConstraintManager<T> scope, object other) {
      var comparer = new CompareLogic();
      return scope.Matches(thisObject => comparer.Compare(thisObject, other).AreEqual);
    }

    public static T HasState<T, TState>(this IArgumentConstraintManager<T> manager, TState other)
      where T : AggregateRoot<TState> {
      var comparer = new CompareLogic();
      return manager.Matches(thisObject => comparer.Compare(thisObject.Deflate(), other).AreEqual);
    }
  }
}