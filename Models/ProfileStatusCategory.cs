using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerConsumer.Models
{
    [Table("profile_status_category", Schema = "public")]
    public class ProfileStatusCategory
    {
        #region snippet_Properties

        [Column("id")]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Column("name")]
        public string Name { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }
}