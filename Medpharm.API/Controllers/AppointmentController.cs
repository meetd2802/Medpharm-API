using Medpharm.Services.IService;
using Medpharm.BusinessModels.Models;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("getallappointments")]
        [Produces("application/json")]
        public IActionResult GetAllAppointments()
        {
            var response = _appointmentService.GetAllAppointments();
            return response.Send();
        }

        [HttpGet("getappointment/{appointmentId}")]
        public IActionResult GetAppointmentById([FromRoute] int appointmentId)
        {
            var appointment = _appointmentService.GetAppointmentById(appointmentId);

            if (appointment == null) // Ensure the service didn't return null
            {
                return NotFound(new { message = $"Appointment with ID {appointmentId} not found." });
            }

            return Ok(appointment); // Return HTTP 200 with the found appointment
        }

        [HttpPost("createappointment")]
        public IActionResult CreateAppointment([FromForm] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest(new { message = "Invalid appointment data." });
            }

            try
            {
                // ✅ Razorpay Order Creation
                var client = new RazorpayClient("rzp_test_jWPrsJGpszeYSa", "ht7h4uRGab7tQwvRZmXkeZOv");

                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", 500 }, // ₹500 in paise
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString().Substring(0, 20) }, // Receipt should be max 40 chars
                    { "payment_capture", 1 }
                };

                var order = client.Order.Create(options);

                // You can optionally store this order ID to the appointment object before saving
                appointment.OrderId = order["id"].ToString();

                // 🔄 Save appointment to DB (your existing logic)
                var response = _appointmentService.CreateAppointment(appointment);

                // ✅ Return response + Razorpay order info
                return Ok(new
                {
                    success = true,
                    message = "Appointment created",
                    data = response.Data,
                    razorpay = new
                    {
                        orderId = order["id"].ToString(),
                        amount = order["amount"],
                        currency = order["currency"],
                        key = "rzp_test_jWPrsJGpszeYSa"
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating appointment.", error = ex.Message });
            }
        }

        [HttpPut("updateappointment")]
        public IActionResult UpdateAppointment([FromBody] Appointment updatedAppointment)
        {
            if (updatedAppointment == null)
            {
                return BadRequest(new { message = "Invalid appointment data: Request body is missing." });
            }

            try
            {
                var existingAppointment = _appointmentService.GetAppointmentById(updatedAppointment.AppointmentId);
                if (existingAppointment == null)
                {
                    return NotFound(new { message = $"Appointment with ID {updatedAppointment.AppointmentId} not found." });
                }

                var response = _appointmentService.UpdateAppointment(updatedAppointment);
                return Ok(new { message = "Appointment updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the appointment.", error = ex.Message });
            }
        }
        
        

        [HttpDelete("deleteappointment/{id}")]
        public IActionResult DeleteAppointment([FromRoute] int id)
        {
            var response = _appointmentService.DeleteAppointment(id);
            return response.Send();
        }

        [HttpGet("getappointmentsbyphone/{phone}")]
        public IActionResult GetAppointmentsByPhone([FromRoute] string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest(new { message = "Phone number is required." });
            }

            var response = _appointmentService.GetAppointmentsByPhone(phone);
            return response.Send();
        }
    }
}
