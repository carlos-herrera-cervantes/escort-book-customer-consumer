using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscortBookCustomerConsumer.Models;

[Table("profile", Schema = "public")]
public class Profile
{
    #region snippet_Properties

    [Column("id")]
    public string ID { get; set; } = Guid.NewGuid().ToString();

    [Column("customer_id")]
    [Required]
    public string CustomerID { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; } = "empty";

    [Column("last_name")]
    public string LastName { get; set; } = "empty";

    [Column("email")]
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; } = "empty";

    [Column("gender")]
    public string Gender { get; set; } = "NotSpecified";

    [Column("birthdate")]
    public DateTime Birthdate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    #endregion
}
