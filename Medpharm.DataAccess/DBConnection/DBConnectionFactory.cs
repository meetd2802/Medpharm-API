using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Medpharm.BusinessModels.Models;
using Medpharm.Common;
using Newtonsoft.Json;

namespace Medpharm.DataAccess.DBConnection
{
    public class DBConnectionFactory : DbContext
    {
        //add dbconnection here.
        static readonly string connectionString = "Server=localhost; User ID=root; Password=meet123; Database=Medpharm";
  
        public DbSet<Appointment> appointment { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<MedicalUpdate> medicalUpdates { get; set; } // Added MedicalUpdate
        public DbSet<Patient> patients { get; set; }
        
        public DbSet<HealthTip> health_tips { get; set; }
        
        public DbSet<Admin> admins { get; set; }
        
        public DbSet<Service> services { get; set; }
        
        public DbSet<Waitlist> waitlists { get; set; }
        public DbSet<ContactForm> contactForms { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });
        }

        public class Appointment
        {
            public int AppointmentId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Diseases { get; set; }
            public string History { get; set; }
            public string Medicine { get; set; }
            public string LabReport { get; set; }
            public string Doctor { get; set; }
            public DateTime AppointmentTime { get; set; }
            public string PaymentId { get; set; }
            public string OrderId { get; set; }
            public string Signature { get; set; }
            
            public string Status { get; set; }
        }
        
        public class Doctor
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Speciality { get; set; }
            public string ImagePath { get; set; } // Path for storing the doctor's image
        }

        public class MedicalUpdate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImagePath { get; set; } // Changed Image to ImagePath
            public string Description { get; set; }
        }
        
        public class Patient
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(255)]
            public string Name { get; set; }

            [Required]
            [StringLength(20)]
            public string Phone { get; set; }

            public string Diseases { get; set; }

            public string History { get; set; }

            public string Medicine { get; set; }

            [Column("lab_report")] // Ensures correct mapping to MySQL column
            public string LabReport { get; set; }

            public string Surgery { get; set; }

            [Required]
            [StringLength(10)]
            public string Gender { get; set; }

            public decimal? Weight { get; set; }

            public string Prescription { get; set; }

            public string Reports { get; set; }

            [Column("created_at")]  // Fix case sensitivity issue
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }
        
        [Table("admin")]
        public class Admin
        {
            [Key]
            [Column("id")]
            public int Id { get; set; }

            [Required]
            [Column("full_name")]
            [StringLength(255)]
            public string FullName { get; set; }

            [Required]
            [Column("user_name")]
            [StringLength(255)]
            public string UserName { get; set; }

            [Required]
            [Column("email")]
            [StringLength(255)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Column("role")]
            [StringLength(100)]
            public string Role { get; set; }
        
            [Required]
            [Column("phone")]
            [StringLength(20)]
            public string Phone { get; set; }

            [Required]
            [Column("password")]
            [StringLength(255)]
            public string Password { get; set; }
        }
        
        public class ForgotPasswordRequest
        {
            public string Username { get; set; }
        }
        
        public class Service
        {
            [Key]
            public int ServiceID { get; set; }

            [Required]
            [StringLength(100)]
            public string ServiceName { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            [StringLength(255)]
            public string ImagePath { get; set; }
        }
        
        [Table("health_tips")]
        public class HealthTip
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [StringLength(255)]
            public string Category { get; set; }  // Nutrition, etc.

            [Required]
            [StringLength(255)]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }  // Text data type

            [StringLength(255)]
            [Column("image_url")] 
            public string ImageUrl { get; set; }  // Path to the image

            [Required]
            [StringLength(255)]
            public string Author { get; set; }

            [Required]
            [Column("date_posted")] // Change here: map the property to the column
            public DateTime DatePosted { get; set; }

            // Automatically populated by the database when inserting a record
            [Column("created_at")]
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            // Automatically updated whenever the record is updated
            [Column("updated_at")]
            public DateTime UpdatedAt { get; set; } = DateTime.Now;
        }
        
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
}