using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using EscortBookCustomer.Consumer.Contexts;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Backgrounds;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<EscortBookCustomerConsumerContext>(options
    => options.UseNpgsql(Environment.GetEnvironmentVariable("PG_DB_CONNECTION")));
builder.Services.AddTransient<IProfileStatusCategoryRepository, ProfileStatusCategoryRepository>();
builder.Services.AddTransient<IProfileStatusRepository, ProfileStatusRepository>();
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddHostedService<KafkaCustomerConsumer>();
builder.Services.AddHostedService<KafkaActiveAccountConsumer>();

var app = builder.Build();

app.Run();
