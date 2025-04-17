using Medpharm.Services.IService;
using Medpharm.BusinessModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class MedicalUpdateController : Controller
    {
        private readonly IMedicalUpdateService _medicalUpdateService;

        public MedicalUpdateController(IMedicalUpdateService medicalUpdateService)
        {
            _medicalUpdateService = medicalUpdateService;
        }

        [HttpGet("getallmedicalupdates")]
        [Produces("application/json")]
        public IActionResult GetAllMedicalUpdates()
        {
            var response = _medicalUpdateService.GetAllMedicalUpdates();
            return response.Send();
        }

        [HttpGet("getmedicalupdate/{id}")]
        public IActionResult GetMedicalUpdateById([FromRoute] int id)
        {
            var medicalUpdate = _medicalUpdateService.GetMedicalUpdateById(id);

            if (medicalUpdate == null)
            {
                return NotFound(new { message = $"Medical update with ID {id} not found." });
            }

            return Ok(medicalUpdate);
        }

        [HttpPost("createmedicalupdate")]
public IActionResult CreateMedicalUpdate([FromForm] string name, [FromForm] string description, [FromForm] IFormFile MedicalUpdateImage)
{
    try
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
        {
            return BadRequest(new { message = "Medical update name and description are required." });
        }

        string imagePath = null;

        if (MedicalUpdateImage != null && MedicalUpdateImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "MedicalUpdates");

            // ✅ Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                Console.WriteLine($"✔ Created directory: {uploadsFolder}");
            }

            // ✅ Use original file name
            string uniqueFileName = Path.GetFileName(MedicalUpdateImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                MedicalUpdateImage.CopyTo(stream);
            }

            imagePath = $"/Images/MedicalUpdates/{uniqueFileName}"; // ✅ Ensure correct return path
            Console.WriteLine($"✔ Image saved at: {filePath}"); // ✅ Debugging log
        }

        var medicalUpdate = new MedicalUpdate
        {
            Name = name,
            Description = description,
            ImagePath = imagePath
        };

        var response = _medicalUpdateService.CreateMedicalUpdate(medicalUpdate);

        Console.WriteLine($"✔ Medical Update Created: {medicalUpdate.Name}, Image: {imagePath}"); // ✅ Debugging log

        return Ok(new { 
            success = true, 
            message = "Medical update created successfully!", 
            data = response, 
            imagePath = imagePath  
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}"); // ✅ Debugging log
        return StatusCode(500, new { message = "Error creating medical update.", error = ex.Message });
    }
}

        [HttpPut("updatemedicalupdate")]
        public IActionResult UpdateMedicalUpdate([FromForm] MedicalUpdate updatedMedicalUpdate, IFormFile? medicalUpdateImage)
        {
            Console.WriteLine($"Updating Medical Update: ID={updatedMedicalUpdate.Id}, Name={updatedMedicalUpdate.Name}, Description={updatedMedicalUpdate.Description}, ImagePath={updatedMedicalUpdate.ImagePath}");

            if (updatedMedicalUpdate == null)
            {
                return BadRequest(new { message = "Invalid medical update data: Request body is missing." });
            }

            var existingMedicalUpdate = _medicalUpdateService.GetMedicalUpdateById(updatedMedicalUpdate.Id);
            if (existingMedicalUpdate == null)
            {
                return NotFound(new { message = $"Medical Update with ID {updatedMedicalUpdate.Id} not found." });
            }

            if (medicalUpdateImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + medicalUpdateImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    medicalUpdateImage.CopyTo(fileStream);
                }

                updatedMedicalUpdate.ImagePath = "/Images/" + uniqueFileName; // ✅ Save new image path
            }
    
            _medicalUpdateService.UpdateMedicalUpdate(updatedMedicalUpdate);
            return Ok(new { message = "Medical update updated successfully.", data = updatedMedicalUpdate });
        }

        [HttpDelete("deletemedicalupdate/{id}")]
        public IActionResult DeleteMedicalUpdate([FromRoute] int id)
        {
            var response = _medicalUpdateService.DeleteMedicalUpdate(id);
            return response.Send();
        }
    }
}
