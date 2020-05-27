using System.Linq;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Data {
  public class DefaultCosmosLinqQuery : ICosmosLinqQuery {
    public FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query) {
      return query.ToFeedIterator();
    }
  }
}