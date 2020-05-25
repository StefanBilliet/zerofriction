using System.Threading.Tasks;

namespace WebApi.Commands {
  public interface ICommandHandler<in T> {
    public Task Handle(T command);
  }
}