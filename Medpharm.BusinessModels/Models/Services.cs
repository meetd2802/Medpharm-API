using System.ComponentModel.DataAnnotations;

namespace Medpharm.BusinessModels.Models
{


    public class Service
    {
        [Key] public int ServiceID { get; set; }

        [Required] [StringLength(100)] public string ServiceName { get; set; }

        [Required] public string Description { get; set; }

        [Required] [StringLength(255)] public string ImagePath { get; set; }
    }

}