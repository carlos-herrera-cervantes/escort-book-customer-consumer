using System;

namespace EscortBookCustomerConsumer.Models
{
    public class ProfileStatusCategory
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }
}