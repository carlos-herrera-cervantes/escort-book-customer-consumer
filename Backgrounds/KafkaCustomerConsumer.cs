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

namespace EscortBookCustomerConsumer.Backgrounds
{
    public class KafkaCustomerConsumer : BackgroundService
    {
        #region snippet_Properties

        private readonly IConfiguration _configuration;

        private readonly IProfileRepository _profileRepository;

        private readonly IProfileStatusRepository _profileStatusRepository;

        private readonly IProfileStatusCategoryRepository _profileStatusCategoryRepository;

        #endregion

        #region snippet_Constructor

        public KafkaCustomerConsumer(IConfiguration configuration, IServiceScopeFactory factory)
        {
            _configuration = configuration;
            _profileRepository = factory
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IProfileRepository>();
            
            _profileStatusRepository = factory
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<IProfileStatusRepository>();

            _profileStatusCategoryRepository = factory
                .CreateScope()
                .ServiceProvider
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
            builder.Subscribe(_configuration["Kafka:Topic"]);

            var cancelToken = new CancellationTokenSource();
            var createdStatus = await _profileStatusCategoryRepository.GetByName("Created");

            while (!stoppingToken.IsCancellationRequested)
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
                        ProfileID = newProfile.ID,
                        ProfileStatusCategoryID = createdStatus.ID
                    };

                    await _profileStatusRepository.CreateAsync(newProfileStatus);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"AN ERROR HAS OCURRED: {e.Message}");
                    builder.Close();
                }
            }
        }

        #endregion
    }
}
