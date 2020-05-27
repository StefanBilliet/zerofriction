using System;
using System.Linq;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApi;
using WebApi.Controllers;

namespace Tests {
  public class CompositionTestsFixture : IDisposable {
    public IServiceProvider Provider { get; }
    
    public CompositionTestsFixture() {
      var serviceCollection = new ServiceCollection();
      var controllers = typeof(CustomersController).Assembly.ExportedTypes
        .Where(x => !x.IsAbstract && typeof(ControllerBase).IsAssignableFrom(x)).ToList();
      controllers.ForEach(c => serviceCollection.AddTransient(c));
      new Startup(new ConfigurationBuilder().Build()).ConfigureServices(serviceCollection);
      SetUpFakeDependencies(serviceCollection);
      Provider = serviceCollection.BuildServiceProvider();
    }

    private static void SetUpFakeDependencies(ServiceCollection serviceCollection) {
      serviceCollection.Replace(new ServiceDescriptor(typeof(CosmosClient), A.Fake<CosmosClient>()));
    }

    public void Dispose() {
    }
  }
}