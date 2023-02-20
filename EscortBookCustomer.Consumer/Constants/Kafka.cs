using System;

namespace EscortBookCustomer.Consumer.Constants;

public static class KafkaTopic
{
    public const string CustomerCreated = "customer-created";

    public const string UserActiveAccount = "user-active-account";
}

public static class KafkaClient
{
    public static readonly string GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID");

    public static readonly string Servers = Environment.GetEnvironmentVariable("KAFKA_SERVERS");
}
