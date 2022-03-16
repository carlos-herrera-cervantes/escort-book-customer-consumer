using System;

namespace EscortBookCustomerConsumer.Models
{
    public class ProfileStatus
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string ProfileID { get; set; }

        public string ProfileStatusCategoryID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }
}