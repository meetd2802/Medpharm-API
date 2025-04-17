using Medpharm.Services.IService;
using Medpharm.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactFormController : Controller
    {
        private readonly IContactFormService _contactFormService;

        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        // Get all contact form entries
        [HttpGet("getallcontactforms")]
        [Produces("application/json")]
        public IActionResult GetAllContactForms()
        {
            var response = _contactFormService.GetAllContactForms();
            return response.Send();
        }

        // Get a specific contact form entry by ID
        [HttpGet("getcontactform/{id}")]
        public IActionResult GetContactFormById([FromRoute] int id)
        {
            var contactFormEntry = _contactFormService.GetContactFormById(id);

            if (contactFormEntry == null)
            {
                return NotFound(new { message = $"Contact form entry with ID {id} not found." });
            }

            return Ok(contactFormEntry);
        }

        // Create a new contact form entry
        [HttpPost("createcontactform")]
        public IActionResult CreateContactForm([FromBody] ContactForm contactForm)
        {
            if (contactForm == null)
            {
                return BadRequest(new { message = "Invalid contact form data." });
            }

            try
            {
                var response = _contactFormService.CreateContactForm(contactForm);
                return response.Send();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating contact form entry.", error = ex.Message });
            }
        }

        // Update an existing contact form entry
        [HttpPut("updatecontactform")]
        public IActionResult UpdateContactForm([FromBody] ContactForm updatedContactForm)
        {
            if (updatedContactForm == null)
            {
                return BadRequest(new { message = "Invalid contact form data: Request body is missing." });
            }

            try
            {
                var existingContactForm = _contactFormService.GetContactFormById(updatedContactForm.Id);
                if (existingContactForm == null)
                {
                    return NotFound(new { message = $"Contact form entry with ID {updatedContactForm.Id} not found." });
                }

                var response = _contactFormService.UpdateContactForm(updatedContactForm);
                return Ok(new { message = "Contact form entry updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the contact form entry.", error = ex.Message });
            }
        }

        // Delete a contact form entry
        [HttpDelete("deletecontactform/{id}")]
        public IActionResult DeleteContactForm([FromRoute] int id)
        {
            var response = _contactFormService.DeleteContactForm(id);
            return response.Send();
        }
    }
}
