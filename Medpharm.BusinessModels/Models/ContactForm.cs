using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medpharm.Models
{
    [Table("contact_form")] // Table name in the database
    public class ContactForm
    {
        [Key] // Primary Key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment for the ID
        public int Id { get; set; } // Unique identifier for each form submission

        [Column("full_name")]
        [Required] // Making Full Name required
        [StringLength(255)] // Limiting the length to 255 characters
        public string FullName { get; set; } // Full Name of the person

        [Column("email")]
        [Required] // Making Email required
        [StringLength(255)] // Limiting the length to 255 characters
        [EmailAddress] // Ensuring the email is valid
        public string Email { get; set; } // Email address of the person
        
        [Column("subject")]
        [StringLength(255)] // Limiting the length of the subject to 255 characters
        public string Subject { get; set; } // Subject of the message (optional)

        [Column("message")]
        [Required] // Making Message required
        public string Message { get; set; } // Message content (TEXT type to allow longer messages)

        [Column("created_at")] // Maps to created_at column in the database
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value of current timestamp
    }
}