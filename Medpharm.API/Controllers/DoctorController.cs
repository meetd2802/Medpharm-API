using Medpharm.Services.IService;
using Medpharm.BusinessModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("getalldoctors")]
        [Produces("application/json")]
        public IActionResult GetAllDoctors()
        {
            var response = _doctorService.GetAllDoctors();
            return response.Send();
        }

        [HttpGet("getdoctor/{doctorId}")]
        public IActionResult GetDoctorById([FromRoute] int doctorId)
        {
            var doctor = _doctorService.GetDoctorById(doctorId);

            if (doctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {doctorId} not found." });
            }

            return Ok(doctor);
        }

        [HttpPost("createdoctor")]
public IActionResult CreateDoctor([FromForm] string name, [FromForm] string speciality, [FromForm] IFormFile DoctorImage)
{
    try
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(speciality))
        {
            return BadRequest(new { message = "Doctor name and speciality are required." });
        }

        string imagePath = null;

        if (DoctorImage != null && DoctorImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

            // ✅ Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
                Console.WriteLine($"✔ Created directory: {uploadsFolder}");
            }

            // ✅ Use original file name
            string uniqueFileName = Path.GetFileName(DoctorImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                DoctorImage.CopyTo(stream);
            }

            imagePath = $"/Images/{uniqueFileName}"; // ✅ Ensure correct return path
            Console.WriteLine($"✔ Image saved at: {filePath}"); // ✅ Debugging log
        }

        var doctor = new Doctor
        {
            Name = name,
            Speciality = speciality,
            ImagePath = imagePath
        };

        var response = _doctorService.CreateDoctor(doctor);

        Console.WriteLine($"✔ Doctor Created: {doctor.Name}, Image: {imagePath}"); // ✅ Debugging log

        return Ok(new { 
            success = true, 
            message = "Doctor created successfully!", 
            data = response, 
            imagePath = imagePath  
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}"); // ✅ Debugging log
        return StatusCode(500, new { message = "Error creating doctor.", error = ex.Message });
    }
}

        [HttpPut("updatedoctor")]
        public IActionResult UpdateDoctor([FromForm] Doctor updatedDoctor, IFormFile? doctorImage)
        {
            Console.WriteLine($"Updating Doctor: ID={updatedDoctor.Id}, Name={updatedDoctor.Name}, Speciality={updatedDoctor.Speciality}, ImagePath={updatedDoctor.ImagePath}");

            if (updatedDoctor == null)
            {
                return BadRequest(new { message = "Invalid doctor data: Request body is missing." });
            }

            var existingDoctor = _doctorService.GetDoctorById(updatedDoctor.Id);
            if (existingDoctor == null)
            {
                return NotFound(new { message = $"Doctor with ID {updatedDoctor.Id} not found." });
            }

            if (doctorImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + doctorImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    doctorImage.CopyTo(fileStream);
                }

                updatedDoctor.ImagePath = "/Images/" + uniqueFileName; // ✅ Save new image path
            }
            
            _doctorService.UpdateDoctor(updatedDoctor);
            return Ok(new { message = "Doctor updated successfully.", data = updatedDoctor });
        }


        [HttpDelete("deletedoctor/{id}")]
        public IActionResult DeleteDoctor([FromRoute] int id)
        {
            var response = _doctorService.DeleteDoctor(id);
            return response.Send();
        }
    }
}
