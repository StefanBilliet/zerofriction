using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Tests.Infrastructure.CosmosDbTestingUtilities {
  public class DummyFeedIterator<T> : FeedIterator<T> {
    private readonly ImmutableList<T> _items;
    private int _currentIndex;

    public DummyFeedIterator(IQueryable<T> query) {
      _items = query.ToImmutableList();
      _currentIndex = 0;
    }

    public override Task<FeedResponse<T>> ReadNextAsync(CancellationToken cancellationToken = new CancellationToken()) {
      var readNextAsync = Task.FromResult((FeedResponse<T>)new DummyFeedResponse<T>(new List<T> {_items[_currentIndex]}));
      _currentIndex++;
      return readNextAsync;
    }

    public override bool HasMoreResults => _items.Count > _currentIndex;
  }
}