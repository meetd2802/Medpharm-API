using Medpharm.Services.IService;
using Medpharm.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("getallpatients")]
        [Produces("application/json")]
        public IActionResult GetAllPatients()
        {
            var response = _patientService.GetAllPatients();
            return response.Send();
        }

        [HttpGet("getpatient/{patientId}")]
        public IActionResult GetPatientById([FromRoute] int patientId)
        {
            var patient = _patientService.GetPatientById(patientId);

            if (patient == null)
            {
                return NotFound(new { message = $"Patient with ID {patientId} not found." });
            }

            return Ok(patient);
        }

        [HttpPost("createpatient")]
        public IActionResult CreatePatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest(new { message = "Invalid patient data." });
            }

            try
            {
                var response = _patientService.CreatePatient(patient);
                return response.Send();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating patient.", error = ex.Message });
            }
        }

        [HttpPut("updatepatient")]
        public IActionResult UpdatePatient([FromBody] Patient updatedPatient)
        {
            if (updatedPatient == null)
            {
                return BadRequest(new { message = "Invalid patient data: Request body is missing." });
            }

            try
            {
                var existingPatient = _patientService.GetPatientById(updatedPatient.Id);
                if (existingPatient == null)
                {
                    return NotFound(new { message = $"Patient with ID {updatedPatient.Id} not found." });
                }

                var response = _patientService.UpdatePatient(updatedPatient);
                return Ok(new { message = "Patient updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the patient.", error = ex.Message });
            }
        }

        [HttpDelete("deletepatient/{id}")]
        public IActionResult DeletePatient([FromRoute] int id)
        {
            var response = _patientService.DeletePatient(id);
            return response.Send();
        }

        [HttpGet("search")]
        [Produces("application/json")]
        public IActionResult SearchPatients([FromQuery] string keyword = "", [FromQuery] string gender = "")
        {
            try
            {
                var response = _patientService.SearchPatients(keyword, gender);
                return response.Send();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching patients.", error = ex.Message });
            }
        }
    }
}
