using System;
using System.Runtime.Serialization;

namespace Domain.Exceptions {
  [Serializable]
  public class AggregateNotFoundException<TAggregate> : Exception {
    public AggregateNotFoundException() {
    }

    public AggregateNotFoundException(int id) : base($"No {typeof(TAggregate)} found with id {id}") {
    }

    public AggregateNotFoundException(string message) : base(message) {
    }

    public AggregateNotFoundException(string message, Exception innerException) : base(message, innerException) {
    }

    protected AggregateNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) {
    }
  }
}