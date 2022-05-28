using EscortBookCustomerConsumer.Backgrounds;
using EscortBookCustomerConsumer.Contexts;
using EscortBookCustomerConsumer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EscortBookCustomerConsumer
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EscortBookCustomerConsumerContext>(options
                => options.UseNpgsql(Configuration["ConnectionStrings:Default"]));
            services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
            services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddHostedService<KafkaCustomerConsumer>();
            services.AddHostedService<KafkaActiveAccountConsumer>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {}
    }
}
