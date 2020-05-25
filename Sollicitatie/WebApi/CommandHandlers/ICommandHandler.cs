using System.Threading.Tasks;

namespace WebApi.CommandHandlers {
  public interface ICommandHandler<in T> {
    public Task Handle(T command);
  }
}