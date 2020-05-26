using System.Threading.Tasks;
using Data.Repositories;
using Domain;
using Domain.State;

namespace Tests.Commands.CreateCustomer {
  public class DummyCustomerRepository : ICustomerRepository {
    public CustomerState? UpsertedCustomer { get; set; }

    public Task Upsert(Customer customer) {
      UpsertedCustomer = customer.Deflate();
      return Task.CompletedTask;
    }
  }
}