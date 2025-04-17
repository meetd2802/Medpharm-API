namespace Medpharm.BusinessModels.Models
{
    public class MedicalUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; } // Path to store update image
        public string Description { get; set; }
    }
}