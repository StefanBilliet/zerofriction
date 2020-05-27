using System.Threading.Tasks;

namespace WebApi.Commands.Shared {
  public interface ICommandHandler<in T> {
    public Task Handle(T command);
  }
}