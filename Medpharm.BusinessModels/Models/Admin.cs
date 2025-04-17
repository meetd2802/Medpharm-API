using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medpharm.BusinessModels.Models
{
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
}