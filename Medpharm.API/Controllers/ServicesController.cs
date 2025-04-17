using Medpharm.Services.IService;
using Medpharm.BusinessModels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet("getallservices")]
        [Produces("application/json")]
        public IActionResult GetAllServices()
        {
            var response = _serviceService.GetAllServices();
            return response.Send();
        }

        [HttpGet("getservice/{serviceId}")]
        public IActionResult GetServiceById([FromRoute] int serviceId)
        {
            var service = _serviceService.GetServiceById(serviceId);

            if (service == null)
            {
                return NotFound(new { message = $"Service with ID {serviceId} not found." });
            }

            return Ok(service);
        }

        [HttpPost("createservice")]
        public IActionResult CreateService([FromForm] string serviceName, [FromForm] string description, [FromForm] IFormFile serviceImage)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(description))
                {
                    return BadRequest(new { message = "Service name and description are required." });
                }

                string imagePath = null;

                if (serviceImage != null && serviceImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        Console.WriteLine($"✔ Created directory: {uploadsFolder}");
                    }

                    string uniqueFileName = Path.GetFileName(serviceImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        serviceImage.CopyTo(stream);
                    }

                    imagePath = $"/Images/{uniqueFileName}";
                    Console.WriteLine($"✔ Image saved at: {filePath}");
                }

                var service = new Service
                {
                    ServiceName = serviceName,
                    Description = description,
                    ImagePath = imagePath
                };

                var response = _serviceService.CreateService(service);

                Console.WriteLine($"✔ Service Created: {service.ServiceName}, Image: {imagePath}");

                return Ok(new
                {
                    success = true,
                    message = "Service created successfully!",
                    data = response,
                    imagePath = imagePath
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating service.", error = ex.Message });
            }
        }

        [HttpPut("updateservice")]
        public IActionResult UpdateService([FromForm] Service updatedService, IFormFile? serviceImage)
        {
            Console.WriteLine($"Updating Service: ID={updatedService.ServiceID}, Name={updatedService.ServiceName}, Description={updatedService.Description}, ImagePath={updatedService.ImagePath}");

            if (updatedService == null)
            {
                return BadRequest(new { message = "Invalid service data: Request body is missing." });
            }

            var existingService = _serviceService.GetServiceById(updatedService.ServiceID);
            if (existingService == null)
            {
                return NotFound(new { message = $"Service with ID {updatedService.ServiceID} not found." });
            }

            if (serviceImage != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + serviceImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    serviceImage.CopyTo(fileStream);
                }

                updatedService.ImagePath = "/Images/" + uniqueFileName;
            }

            _serviceService.UpdateService(updatedService);
            return Ok(new { message = "Service updated successfully.", data = updatedService });
        }

        [HttpDelete("deleteservice/{id}")]
        public IActionResult DeleteService([FromRoute] int id)
        {
            var response = _serviceService.DeleteService(id);
            return response.Send();
        }
    }
}
