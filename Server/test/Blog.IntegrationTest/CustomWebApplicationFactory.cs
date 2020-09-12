using Medium.App;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Services;
using Medium.Infrastructure.unitOfWork;
using Medium.IntegrationTest.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Medium.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DataContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                        options.ConfigureWarnings(x => x
                            .Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });

                    var providerDbContext = services.BuildServiceProvider()
                        .GetRequiredService<DataContext>();
                    providerDbContext.SeedTestData();

                    services.AddSingleton<IUnitOfWork>(new UnitOfWork(providerDbContext));
                    services.AddScoped<IAuthorService, AuthorService>();
                    services.AddScoped<IPostService, PostService>();
                    services.AddScoped<IAuthorAuthenticationService, AuthorAuthenticationService>();
                });
        }
    }
}
