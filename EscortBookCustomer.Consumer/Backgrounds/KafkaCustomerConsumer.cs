using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Confluent.Kafka;
using Newtonsoft.Json;
using EscortBookCustomer.Consumer.Types;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Models;
using EscortBookCustomer.Consumer.Constants;

namespace EscortBookCustomer.Consumer.Backgrounds;

public class KafkaCustomerConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly IProfileRepository _profileRepository;

    private readonly IProfileStatusRepository _profileStatusRepository;

    private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

    private readonly ILogger _logger;

    #endregion

    #region snippet_Constructor

    public KafkaCustomerConsumer(ILogger<KafkaCustomerConsumer> logger, IServiceScopeFactory factory)
    {
        var serviceProvider = factory.CreateScope().ServiceProvider;

        _logger = logger;
        _profileRepository = serviceProvider.GetRequiredService<IProfileRepository>();
        _profileStatusRepository = serviceProvider.GetRequiredService<IProfileStatusRepository>();
        _profileStatusCategoryRepository = serviceProvider.GetRequiredService<IProfileStatusCategoryRepository>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID"),
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVERS"),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var builder = new ConsumerBuilder<Ignore, string>(config).Build();
        builder.Subscribe(KafkaTopic.CustomerCreated);

        var cancelToken = new CancellationTokenSource();
        var createdStatus = await _profileStatusCategoryRepository.GetByName("Created");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CreateProfileStatus(builder, cancelToken, createdStatus.ID);
        }
    }

    #endregion

    #region snippet_Helpers

    private async Task CreateProfileStatus
    (
        IConsumer<Ignore, string> builder,
        CancellationTokenSource cancelToken,
        string statusId
    )
    {
        try
        {
            var consumer = builder.Consume(cancelToken.Token);
            var kafkaUserEvent = JsonConvert.DeserializeObject<KafkaUserEvent>(consumer.Message.Value);

            var newProfile = new Profile
            {
                CustomerID = kafkaUserEvent.Id,
                Email = kafkaUserEvent.Email
            };

            await _profileRepository.CreateAsync(newProfile);

            var newProfileStatus = new ProfileStatus
            {
                CustomerID = kafkaUserEvent.Id,
                ProfileStatusCategoryID = statusId
            };

            await _profileStatusRepository.CreateAsync(newProfileStatus);
        }
        catch (Exception e)
        {
            _logger.LogError("AN ERROR HAS OCCURRED CREATING A CUSTOMER");
            _logger.LogError(e.Message);
            builder.Close();
        }
    }

    #endregion
}
