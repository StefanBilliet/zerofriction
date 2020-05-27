using System.Linq;
using Microsoft.Azure.Cosmos;

namespace Data {
  public interface ICosmosLinqQuery {
    FeedIterator<T> GetFeedIterator<T>(IQueryable<T> query);
  }
}