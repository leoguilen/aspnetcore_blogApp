using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.unitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
                    .BuildServiceProvider())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .EnableSensitiveDataLogging()
                .EnableServiceProviderCaching(false));

            serviceProvider.AddSingleton<IUnitOfWork, UnitOfWork>();

            return serviceProvider
                .BuildServiceProvider();
        }
    }
}
