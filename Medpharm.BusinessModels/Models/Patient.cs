using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medpharm.Models
{
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

          // Fix case sensitivity issue
        [Column("created_at")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}