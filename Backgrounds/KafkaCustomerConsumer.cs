using System.Threading;
using System.Threading.Tasks;
using EscortBookCustomerConsumer.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Confluent.Kafka;
using Newtonsoft.Json;
using EscortBookCustomerConsumer.Repositories;
using EscortBookCustomerConsumer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EscortBookCustomerConsumer.Backgrounds
{
    public class KafkaCustomerConsumer : BackgroundService
    {
        #region snippet_Properties

        private readonly IConfiguration _configuration;

        private readonly IProfileRepository _profileRepository;

        private readonly IProfileStatusRepository _profileStatusRepository;

        private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

        private readonly ILogger _logger;

        #endregion

        #region snippet_Constructor

        public KafkaCustomerConsumer
        (
            IConfiguration configuration,
            ILogger<KafkaCustomerConsumer> logger,
            IServiceScopeFactory factory
        )
        {
            _configuration = configuration;
            _logger = logger;

            var serviceProvider = factory.CreateScope().ServiceProvider;

            _profileRepository = serviceProvider.GetRequiredService<IProfileRepository>();
            _profileStatusRepository = serviceProvider.GetRequiredService<IProfileStatusRepository>();
            _profileStatusCategoryRepository = serviceProvider
                .GetRequiredService<IProfileStatusCategoryRepository>();
        }

        #endregion

        #region snippet_ActionMethods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _configuration["Kafka:GroupId"],
                BootstrapServers = _configuration["Kafka:Servers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var builder = new ConsumerBuilder<Ignore, string>(config).Build();
            builder.Subscribe(_configuration["Kafka:Topics:CustomerCreated"]);

            var cancelToken = new CancellationTokenSource();
            var createdStatus = await _profileStatusCategoryRepository.GetByName("Created");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumer = builder.Consume(cancelToken.Token);
                    var kafkaUserEvent = JsonConvert
                        .DeserializeObject<KafkaUserEvent>(consumer.Message.Value);
                    
                    var newProfile = new Profile
                    {
                        CustomerID = kafkaUserEvent.Id,
                        Email = kafkaUserEvent.Email
                    };

                    await _profileRepository.CreateAsync(newProfile);

                    var newProfileStatus = new ProfileStatus
                    {
                        CustomerID = kafkaUserEvent.Id,
                        ProfileStatusCategoryID = createdStatus.ID
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
        }

        #endregion
    }
}
