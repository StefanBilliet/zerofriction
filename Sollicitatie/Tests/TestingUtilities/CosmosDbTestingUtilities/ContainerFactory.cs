using System.Linq;
using FakeItEasy;
using Microsoft.Azure.Cosmos;

namespace Tests.TestingUtilities.CosmosDbTestingUtilities {
  public static class ContainerFactory {
    public static Container FakeContainer<T>(T[] items) {
      var container = A.Fake<Container>();
      A.CallTo(() => container
          .GetItemLinqQueryable<T>(false, null, null))
        .Returns(new EnumerableQuery<T>(items));
      return container;
    }
  }
}