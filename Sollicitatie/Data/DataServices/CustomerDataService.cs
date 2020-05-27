using System;
using System.Threading.Tasks;

namespace Data.DataServices {
  public interface ICustomerDataService {
    Task<bool> CustomerExists(Guid id);
  }

  public class CustomerDataService : ICustomerDataService {
    public Task<bool> CustomerExists(Guid id) {
      throw new NotImplementedException();
    }
  }
}