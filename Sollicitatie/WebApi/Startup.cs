using Data;
using Data.DataServices;
using Data.Repositories;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Commands.AddContactDetailsForCustomer;
using WebApi.Commands.AddContactDetailsForCustomer.Contracts;
using WebApi.Commands.AddCustomer;
using WebApi.Commands.AddCustomer.Contracts;
using WebApi.Commands.InvoiceCustomer;
using WebApi.Commands.InvoiceCustomer.Contracts;
using WebApi.Commands.Shared;

namespace WebApi {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services
        .AddControllers();
      services.AddTransient<ICommandHandler<AddCustomerCommand>, AddCustomerCommandHandler>();
      services.AddTransient<ICommandHandler<AddContactDetailsForCustomerCommand>, AddContactDetailsForCustomerCommandHandler>();
      services.AddTransient<IValidator<AddCustomerCommand>, AddCustomerCommandValidator>();
      services.AddTransient<IValidator<AddContactDetailsForCustomerCommand>, AddContactDetailsForCustomerCommandValidator>();
      services.AddTransient<IValidator<InvoiceCustomerCommand>, InvoiceCustomerCommandValidator>();
      services.AddTransient<ICustomerRepository, CustomerRepository>();
      services.AddTransient<ICustomerDataService, CustomerDataService>();
      services.AddTransient<ICosmosLinqQuery, DefaultCosmosLinqQuery>();
      services.AddScoped<IDbContext, DbContext>();
      services.AddSingleton(provider => {
        var configurationRoot = provider.GetRequiredService<IConfiguration>();
        var cosmosClient = new CosmosClientBuilder(configurationRoot["ZeroFrictionDatabaseConnectionString"])
          .WithSerializerOptions(new CosmosSerializationOptions
            {PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase})
          .Build();
        return cosmosClient;
      });
      services.AddScoped<ICurrentTenantProvider, CurrentTenantProvider>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}