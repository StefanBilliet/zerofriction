using System.Collections.Generic;
using Microsoft.Azure.Cosmos;

namespace Data {
  public static class FeedIteratorExtensions {
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this FeedIterator<T> iterator) {
      if (iterator == null)
        yield break;

      while (iterator.HasMoreResults) {
        foreach (var item in await iterator.ReadNextAsync().ConfigureAwait(false)) {
          yield return item;
        }
      }
    }
  }
}