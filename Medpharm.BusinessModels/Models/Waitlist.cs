using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medpharm.Models
{
    [Table("waitlist")] // Table name in the database
    public class Waitlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("full_name")] // Explicitly mapping property to the column name
        public string FullName { get; set; } // Full Name of the person

        [Required]
        [StringLength(255)]
        [EmailAddress] // Ensures that the email is valid
        public string Email { get; set; } // Email address of the person

        [Column("phone_number")]
        [Required]
        [StringLength(20)] // You can adjust this length as needed for phone numbers
        public string PhoneNumber { get; set; } // Phone number of the person

        [Column("preferred_doctor")]
        [StringLength(255)] // Preferred doctor can be any string
        public string PreferredDoctor { get; set; }

        [Column("preferred_timeframe")]
        [StringLength(50)] // Preferred timeframe as a string (e.g., "Morning", "Afternoon", etc.)
        public string PreferredTimeframe { get; set; }

        [Column("created_at")] // Maps to the created_at column in the table
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value of current timestamp
    }
}