using System.Threading.Tasks;
using Data.Repositories;
using Domain;
using Domain.State;

namespace WebApi.Tests.Commands.CreateCustomer {
  public class DummyCustomerRepository : ICustomerRepository {
    public CustomerState? UpsertedCustomer { get; set; }

    public Task Upsert(Customer customer) {
      UpsertedCustomer = customer.Deflate();
      return Task.CompletedTask;
    }
  }
}