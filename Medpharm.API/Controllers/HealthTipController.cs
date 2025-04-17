using Medpharm.Services.IService;
using Medpharm.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class HealthTipController : Controller
    {
        private readonly IHealthTipService _healthTipService;

        public HealthTipController(IHealthTipService healthTipService)
        {
            _healthTipService = healthTipService;
        }

        [HttpGet("getallhealthtips")]
        [Produces("application/json")]
        public IActionResult GetAllHealthTips()
        {
            var response = _healthTipService.GetAllHealthTips();
            return response.Send();
        }

        [HttpGet("gethealthtip/{id}")]
        public IActionResult GetHealthTipById([FromRoute] int id)
        {
            var healthTip = _healthTipService.GetHealthTipById(id);

            if (healthTip == null)
            {
                return NotFound(new { message = $"Health tip with ID {id} not found." });
            }

            return Ok(healthTip);
        }

        [HttpPost("createhealthtip")]
        public async Task<IActionResult> CreateHealthTip([FromForm] HealthTipCreateRequest model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid health tip data." });
            }

            try
            {
                // Handle the image file upload
                var file = model.HealthTipImage;
        
                // Check if a file is uploaded
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No image file uploaded." });
                }

                // Define the file path to save the image in the "wwwroot/Images" directory
                var filePath = Path.Combine("wwwroot", "Images", file.FileName);

                // Save the file to the server in the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Map the data from HealthTipCreateRequest to HealthTip model
                var healthTip = new HealthTip
                {
                    Category = model.Category,
                    Title = model.Title,
                    Description = model.Description,
                    Author = model.Author,
                    DatePosted = model.DatePosted,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    // Save the image URL (the relative path where the image will be accessible in the app)
                    ImageUrl = "/Images/" + file.FileName  // This is the URL for the image
                };

                // Pass the HealthTip object to the service to save in the database
                var response = _healthTipService.CreateHealthTip(healthTip);

                // Return the response, you can customize the return based on your service's result.
                return response.Send();
            }
            catch (Exception ex)
            {
                // Log the error and return a failure message
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating health tip.", error = ex.Message });
            }
        }


// Define a custom class for the incoming data with form data
        public class HealthTipCreateRequest
        {
            public string Category { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public IFormFile HealthTipImage { get; set; }
            public string Author { get; set; }
            public DateTime DatePosted { get; set; }
            public string ImageUrl { get; set; }  // To store image URL if needed
        }
        
        [HttpPut("updatehealthtip")]
        public IActionResult UpdateHealthTip([FromForm] HealthTip updatedHealthTip, [FromForm] IFormFile HealthTipImage)
        {
            if (updatedHealthTip == null)
            {
                return BadRequest(new { message = "Invalid health tip data: Request body is missing." });
            }

            try
            {
                // Retrieve the existing health tip from the database
                var existingHealthTip = _healthTipService.GetHealthTipById(updatedHealthTip.Id);
                if (existingHealthTip == null)
                {
                    return NotFound(new { message = $"Health tip with ID {updatedHealthTip.Id} not found." });
                }

                // Handle file upload if a new image is provided
                if (HealthTipImage != null)
                {
                    // Save the new image and set its path in the HealthTip object
                    // Example: Saving the image to a specific path (adjust this as needed)
                    var filePath = Path.Combine("wwwroot/Images", HealthTipImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        HealthTipImage.CopyTo(stream);
                    }
                    updatedHealthTip.ImageUrl = "/Images/" + HealthTipImage.FileName;  // Set the image URL for the health tip
                }

                // Update the health tip with the new data (including the image URL if updated)
                var response = _healthTipService.UpdateHealthTip(updatedHealthTip);

                return Ok(new { message = "Health tip updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the health tip.", error = ex.Message });
            }
        }

        [HttpDelete("deletehealthtip/{id}")]
        public IActionResult DeleteHealthTip([FromRoute] int id)
        {
            var response = _healthTipService.DeleteHealthTip(id);
            return response.Send();
        }
    }
}
