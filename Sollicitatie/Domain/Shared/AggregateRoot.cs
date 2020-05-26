namespace Domain.Shared {
  public abstract class AggregateRoot<TState> : Entity {
    public abstract TState Deflate();
  }
}