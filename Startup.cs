using System;
using EscortBookCustomerConsumer.Backgrounds;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EscortBookCustomerConsumer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("CONNECTION STRING: " + Environment.GetEnvironmentVariable("PG_DB_CONNECTION"));
        services.AddDbContext<EscortBookCustomerConsumerContext>(options
            => options.UseNpgsql(Environment.GetEnvironmentVariable("PG_DB_CONNECTION")));
        services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
        services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
        services.AddTransient<IProfileRepository, ProfileRepository>();
        services.AddHostedService<KafkaCustomerConsumer>();
        services.AddHostedService<KafkaActiveAccountConsumer>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
}
