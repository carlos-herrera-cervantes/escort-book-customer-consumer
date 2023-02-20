using System;

namespace EscortBookCustomer.Consumer.Constants;

public static class PostgresClient
{
    public static readonly string CustomerProfile = Environment.GetEnvironmentVariable("PG_DB_CONNECTION");
}
