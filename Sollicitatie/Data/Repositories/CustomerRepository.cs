using System.Threading.Tasks;
using Domain;

namespace Data.Repositories {
  public interface ICustomerRepository {
    Task Upsert(Customer customer);
  }
  
  public class CustomerRepository : ICustomerRepository {
    public Task Upsert(Customer customer) {
      throw new System.NotImplementedException();
    }
  }
}