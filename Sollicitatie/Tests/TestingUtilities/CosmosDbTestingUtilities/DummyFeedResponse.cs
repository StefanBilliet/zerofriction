using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace Tests.TestingUtilities.CosmosDbTestingUtilities {
  public class DummyFeedResponse<T> : FeedResponse<T> {
    private readonly IEnumerable<T> _items;

    public override Headers? Headers { get; }
    public override IEnumerable<T> Resource => _items;
    public override HttpStatusCode StatusCode { get; }
    public override CosmosDiagnostics? Diagnostics { get; } 
    public override string? ContinuationToken { get; }
    public override int Count => _items.Count();

    public DummyFeedResponse(IEnumerable<T> items) {
      _items = items ?? throw new ArgumentNullException(nameof(items));
    }
     
    public override IEnumerator<T> GetEnumerator() {
      return _items.GetEnumerator();
    }

  }
}