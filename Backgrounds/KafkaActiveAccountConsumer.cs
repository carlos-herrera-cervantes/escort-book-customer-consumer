using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EscortBookCustomerConsumer.Repositories;
using EscortBookCustomerConsumer.Types;
using EscortBookCustomerConsumer.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscortBookCustomerConsumer.Backgrounds;

public class KafkaActiveAccountConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly IProfileStatusRepository _profileStatusRepository;

    private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

    private readonly ILogger _logger;

    #endregion

    #region snippet_Constructors

    public KafkaActiveAccountConsumer(ILogger<KafkaActiveAccountConsumer> logger, IServiceScopeFactory factory)
    {
        var serviceProvider = factory.CreateScope().ServiceProvider;

        _logger = logger;
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
        builder.Subscribe(KafkaTopic.UserActiveAccount);

        var cancelToken = new CancellationTokenSource();
        var activeStatus = await _profileStatusCategoryRepository.GetByName("Active");

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateProfileStatus(builder, cancelToken, activeStatus.ID);
        }
    }

    #endregion

    #region snippet_Helpers

    private async Task UpdateProfileStatus
    (
        IConsumer<Ignore, string> builder,
        CancellationTokenSource cancelToken,
        string statusId
    )
    {
        try
        {
            var consumer = builder.Consume(cancelToken.Token);
            var kafkaActiveAccountEvent = JsonConvert.DeserializeObject<KafkaActiveAccountEvent>(consumer.Message.Value);
            var profileStatus = await _profileStatusRepository.GetByProfileIdAsync(kafkaActiveAccountEvent.UserId);

            if (profileStatus is null) return;

            profileStatus.ProfileStatusCategoryID = statusId;

            await _profileStatusRepository.UpdateAsync(profileStatus);
        }
        catch (Exception e)
        {
            _logger.LogError("AN ERROR HAS OCCURRED ACTIVATING CUSTOMER ACCOUNT:");
            _logger.LogError(e.Message);
            builder.Close();
        }
    }

    #endregion
}
