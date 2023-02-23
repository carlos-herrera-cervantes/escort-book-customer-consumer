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

    private readonly IConsumer<Ignore, string> _consumer;

    #endregion

    #region snippet_Constructor

    public KafkaCustomerConsumer(ILogger<KafkaCustomerConsumer> logger, IServiceScopeFactory factory)
    {
        var serviceProvider = factory.CreateScope().ServiceProvider;

        _logger = logger;
        _profileRepository = serviceProvider.GetRequiredService<IProfileRepository>();
        _profileStatusRepository = serviceProvider.GetRequiredService<IProfileStatusRepository>();
        _profileStatusCategoryRepository = serviceProvider.GetRequiredService<IProfileStatusCategoryRepository>();
        _consumer = serviceProvider.GetRequiredService<IConsumer<Ignore, string>>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(KafkaTopic.CustomerCreated);

        var cancelToken = new CancellationTokenSource();
        var createdStatus = await _profileStatusCategoryRepository.GetByName("Created");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CreateProfileStatus(cancelToken, createdStatus.ID);
        }
    }

    #endregion

    #region snippet_Helpers

    private async Task CreateProfileStatus(CancellationTokenSource cancelToken, string statusId)
    {
        try
        {
            var consumer = _consumer.Consume(cancelToken.Token);
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
            _consumer.Close();
        }
    }

    #endregion
}
