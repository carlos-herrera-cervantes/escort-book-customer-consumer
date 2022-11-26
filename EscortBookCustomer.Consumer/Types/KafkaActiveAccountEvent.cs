using Newtonsoft.Json;

namespace EscortBookCustomer.Consumer.Types;

public class KafkaActiveAccountEvent
{
    [JsonProperty("userId")]
    public string UserId { get; set; }
}
