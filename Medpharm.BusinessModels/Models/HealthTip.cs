using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medpharm.Models
{
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
}