using Newtonsoft.Json;

namespace EscortBookCustomerConsumer.Types;

public class KafkaActiveAccountEvent
{
    [JsonProperty("userId")]
    public string UserId { get; set; }
}
