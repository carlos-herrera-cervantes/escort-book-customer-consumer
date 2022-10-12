using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerConsumer.Models;

[Table("profile_status", Schema = "public")]
public class ProfileStatus
{
    #region snippet_Properties

    [Column("id")]
    public string ID { get; set; } = Guid.NewGuid().ToString();

    [Column("customer_id")]
    public string CustomerID { get; set; }

    [Column("profile_status_category_id")]
    public string ProfileStatusCategoryID { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    #endregion
}
