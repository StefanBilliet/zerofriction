using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Controllers;
using Xunit;

namespace Tests {
  public class CompositionTests : IClassFixture<CompositionTestsFixture> {
    private readonly IServiceProvider _provider;

    public CompositionTests(CompositionTestsFixture fixture) {
      _provider = fixture.Provider;
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Container_can_resolve_all_functions(Type functionType) {
      _provider.GetRequiredService<CustomersController>();
    }

    public static IEnumerable<object[]> TestCases =>
      typeof(CustomersController).Assembly.GetTypes()
        .Where(_ => typeof(ControllerBase).IsAssignableFrom(_))
        .Select(type => new object[] {type});
  }
}