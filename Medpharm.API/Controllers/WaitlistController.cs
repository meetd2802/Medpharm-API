using Medpharm.Services.IService;
using Medpharm.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class WaitlistController : Controller
    {
        private readonly IWaitlistService _waitlistService;

        public WaitlistController(IWaitlistService waitlistService)
        {
            _waitlistService = waitlistService;
        }

        // Get all waitlist entries
        [HttpGet("getallwaitlist")]
        [Produces("application/json")]
        public IActionResult GetAllWaitlist()
        {
            var response = _waitlistService.GetAllWaitlist();
            return response.Send();
        }

        // Get a specific waitlist entry by ID
        [HttpGet("getwaitlist/{id}")]
        public IActionResult GetWaitlistById([FromRoute] int id)
        {
            var waitlistEntry = _waitlistService.GetWaitlistById(id);

            if (waitlistEntry == null)
            {
                return NotFound(new { message = $"Waitlist entry with ID {id} not found." });
            }

            return Ok(waitlistEntry);
        }

        // Create a new waitlist entry
        [HttpPost("createwaitlist")]
        public IActionResult CreateWaitlist([FromBody] Waitlist waitlist)
        {
            if (waitlist == null)
            {
                return BadRequest(new { message = "Invalid waitlist data." });
            }

            try
            {
                var response = _waitlistService.CreateWaitlist(waitlist);
                return response.Send();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating waitlist entry.", error = ex.Message });
            }
        }

        // Update an existing waitlist entry
        [HttpPut("updatewaitlist")]
        public IActionResult UpdateWaitlist([FromBody] Waitlist updatedWaitlist)
        {
            if (updatedWaitlist == null)
            {
                return BadRequest(new { message = "Invalid waitlist data: Request body is missing." });
            }

            try
            {
                var existingWaitlist = _waitlistService.GetWaitlistById(updatedWaitlist.Id);
                if (existingWaitlist == null)
                {
                    return NotFound(new { message = $"Waitlist entry with ID {updatedWaitlist.Id} not found." });
                }

                var response = _waitlistService.UpdateWaitlist(updatedWaitlist);
                return Ok(new { message = "Waitlist entry updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the waitlist entry.", error = ex.Message });
            }
        }

        // Delete a waitlist entry
        [HttpDelete("deletewaitlist/{id}")]
        public IActionResult DeleteWaitlist([FromRoute] int id)
        {
            var response = _waitlistService.DeleteWaitlist(id);
            return response.Send();
        }
    }
}
