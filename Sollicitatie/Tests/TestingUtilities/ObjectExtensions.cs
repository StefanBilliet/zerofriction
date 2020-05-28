using KellermanSoftware.CompareNetObjects;

namespace Tests.TestingUtilities {
  public static class ObjectExtensions {
    public static bool DeepEquals<T>(this T left, T right) {
      var comparer = new CompareLogic();
      return comparer.Compare(left, right).AreEqual;
    }
  }
}