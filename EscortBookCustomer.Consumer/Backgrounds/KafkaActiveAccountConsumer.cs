using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Confluent.Kafka;
using EscortBookCustomer.Consumer.Repositories;
using EscortBookCustomer.Consumer.Types;
using EscortBookCustomer.Consumer.Constants;

namespace EscortBookCustomer.Consumer.Backgrounds;

public class KafkaActiveAccountConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly IProfileStatusRepository _profileStatusRepository;

    private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

    private readonly ILogger _logger;

    private readonly IConsumer<Ignore, string> _consumer;

    #endregion

    #region snippet_Constructors

    public KafkaActiveAccountConsumer(ILogger<KafkaActiveAccountConsumer> logger, IServiceScopeFactory factory)
    {
        var serviceProvider = factory.CreateScope().ServiceProvider;

        _logger = logger;
        _profileStatusRepository = serviceProvider.GetRequiredService<IProfileStatusRepository>();
        _profileStatusCategoryRepository = serviceProvider.GetRequiredService<IProfileStatusCategoryRepository>();
        _consumer = serviceProvider.GetRequiredService<IConsumer<Ignore, string>>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(KafkaTopic.UserActiveAccount);

        var cancelToken = new CancellationTokenSource();
        var activeStatus = await _profileStatusCategoryRepository.GetByName("Active");

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateProfileStatus(cancelToken, activeStatus.ID);
        }
    }

    #endregion

    #region snippet_Helpers

    private async Task UpdateProfileStatus(CancellationTokenSource cancelToken, string statusId)
    {
        try
        {
            var consumer = _consumer.Consume(cancelToken.Token);
            var kafkaActiveAccountEvent = JsonConvert.DeserializeObject<KafkaActiveAccountEvent>(consumer.Message.Value);
            var profileStatus = await _profileStatusRepository.GetAsync(ps => ps.CustomerID == kafkaActiveAccountEvent.UserId);

            if (profileStatus is null) return;

            profileStatus.ProfileStatusCategoryID = statusId;

            await _profileStatusRepository.UpdateAsync(profileStatus);
        }
        catch (Exception e)
        {
            _logger.LogError("AN ERROR HAS OCCURRED ACTIVATING CUSTOMER ACCOUNT:");
            _logger.LogError(e.Message);
            _consumer.Close();
        }
    }

    #endregion
}
