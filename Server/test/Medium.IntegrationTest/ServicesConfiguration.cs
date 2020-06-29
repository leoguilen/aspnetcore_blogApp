using Medium.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Medium.IntegrationTest
{
    public static class ServicesConfiguration
    {
        public static IServiceProvider Configure()
        {
            var serviceProvider = new ServiceCollection();

            serviceProvider.AddDbContext<DataContext>(options => options
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .UseInternalServiceProvider(new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider()));

            return serviceProvider
                .BuildServiceProvider();
        }
    }
}
