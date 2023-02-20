using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Backgrounds;
using EscortBookCustomer.Consumer.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<EscortBookCustomerConsumerContext>(options
    => options.UseNpgsql(PostgresClient.CustomerProfile));
builder.Services.AddKafkaClient(new ConsumerConfig
{
    BootstrapServers = KafkaClient.Servers,
    GroupId = KafkaClient.GroupId
});
builder.Services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
builder.Services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddHostedService<KafkaCustomerConsumer>();
builder.Services.AddHostedService<KafkaActiveAccountConsumer>();

var app = builder.Build();

app.Run();
