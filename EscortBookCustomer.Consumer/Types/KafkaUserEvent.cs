using Newtonsoft.Json;

namespace EscortBookCustomer.Consumer.Types;

public class KafkaUserEvent
{
    [JsonProperty("_id")]
    public string Id { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}
