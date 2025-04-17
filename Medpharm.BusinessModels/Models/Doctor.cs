namespace Medpharm.BusinessModels.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } // Path to store doctor image
        public string Name { get; set; }
        public string Speciality { get; set; }
        
    }
}
