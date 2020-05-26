namespace Infrastructure {
  public interface ICurrentTenantProvider {
    string Get();
  }

  public class CurrentTenantProvider : ICurrentTenantProvider {
    public string Get() {
      //this would normally be retrieved from the current HTTP request, from a header, url segment or querystring, using the IHttpContextAccessor
      return "ACME inc.";
    }
  }
}