using System;
using System.ComponentModel.DataAnnotations;

namespace EscortBookCustomerConsumer.Models
{
    public class Profile
    {
        #region snippet_Properties

        public string ID { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string CustomerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; } = "NotSpecified";

        public DateTime Birthdate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }
}