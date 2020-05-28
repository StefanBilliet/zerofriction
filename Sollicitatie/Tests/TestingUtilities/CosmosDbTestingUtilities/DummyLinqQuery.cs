using System.Linq;
using Data;
using Microsoft.Azure.Cosmos;

namespace Tests.TestingUtilities.CosmosDbTestingUtilities {
  public class DummyLinqQuery : ICosmosLinqQuery {
    public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query) {
      return new DummyFeedIterator<T>(query);
    }
  }
}