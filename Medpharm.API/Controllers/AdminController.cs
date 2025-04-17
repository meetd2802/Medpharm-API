using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Medpharm.Services.IService;
using Medpharm.BusinessModels.Models;
using System;
using System.Threading.Tasks;
using Medpharm.Common;
using Medpharm.DataAccess.DBConnection;
using Medpharm.Services;
using Microsoft.AspNetCore.Cors;

namespace Medpharm.API.Controllers
{
    
    [EnableCors("AllowSpecificOrigin")] 
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IAdminService adminService, IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _adminService = adminService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        // ----------------- ADMIN LOGIN -----------------
        [HttpPost("login")]
        public IActionResult AdminLogin([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var admin = _adminService.AuthenticateAdmin(request.Username, request.Password);

            if (admin == null)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Store admin session
            HttpContext.Session.SetString("AdminId", admin.Id.ToString());
            HttpContext.Session.SetString("AdminUsername", admin.UserName);
            HttpContext.Session.SetString("AdminPhone", admin.Phone ?? "");

            Console.WriteLine($"AdminId: {HttpContext.Session.GetString("AdminId")}");
            Console.WriteLine($"AdminUsername: {HttpContext.Session.GetString("AdminUsername")}");
            Console.WriteLine($"AdminPhone: {HttpContext.Session.GetString("AdminPhone")}"); // ✅ Optional log

            return Ok(new
            {
                success = true,
                message = "Login successful.",
                admin = new
                {
                    admin.Id,
                    admin.FullName,
                    admin.UserName,
                    admin.Email,
                    admin.Role,
                    admin.Phone // ✅ Include phone in response
                }
            });
        }

        // ----------------- ADMIN LOGOUT -----------------
        [HttpPost("logout")]
        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }

        // ----------------- CHECK ADMIN SESSION -----------------
        [HttpGet("checksession")]
        public IActionResult CheckSession()
        {
            var adminId = HttpContext.Session.GetString("AdminId");
            var username = HttpContext.Session.GetString("AdminUsername");
            var phone = HttpContext.Session.GetString("AdminPhone");

            if (string.IsNullOrEmpty(adminId) || string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Session Not Found!");
                return Unauthorized(new { message = "Admin is not logged in." });
            }

            Console.WriteLine($"Session Found - AdminId: {adminId}, Username: {username}, Phone: {phone}");
            return Ok(new { message = "Admin is logged in.", adminId, username, phone });
        }

        // ----------------- EXISTING ENDPOINTS (NO CHANGES) -----------------
        [HttpGet("getalladmins")]
        [Produces("application/json")]
        public IActionResult GetAllAdmins()
        {
            var response = _adminService.GetAllAdmins();
            return response.Send();
        }

        [HttpGet("getadmin/{adminId}")]
        public IActionResult GetAdminById([FromRoute] int adminId)
        {
            var admin = _adminService.GetAdminById(adminId);
            if (admin == null)
            {
                return NotFound(new { message = $"Admin with ID {adminId} not found." });
            }

            return Ok(admin);
        }

        [HttpPost("createadmin")]
        public IActionResult CreateAdmin([FromBody] Admin admin)
        {
            if (admin == null)
            {
                return BadRequest(new { message = "Invalid admin data." });
            }

            try
            {
                var response = _adminService.CreateAdmin(admin);
                if (response.Status != (int)StatusEnum.Success)
                {
                    return StatusCode(500,
                        new { message = response.Message ?? "An error occurred while creating the admin." });
                }

                return Ok(new { message = "Admin created successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error creating admin.", error = ex.Message });
            }
        }

        [HttpPut("updateadmin")]
        public IActionResult UpdateAdmin([FromBody] Admin updatedAdmin)
        {
            if (updatedAdmin == null)
            {
                return BadRequest(new { message = "Invalid admin data: Request body is missing." });
            }

            try
            {
                var existingAdmin = _adminService.GetAdminById(updatedAdmin.Id);
                if (existingAdmin == null)
                {
                    return NotFound(new { message = $"Admin with ID {updatedAdmin.Id} not found." });
                }

                var response = _adminService.UpdateAdmin(updatedAdmin);
                return Ok(new { success = true, message = "Admin updated successfully.", data = response });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500,
                    new { message = "An error occurred while updating the admin.", error = ex.Message });
            }
        }

        [HttpDelete("deleteadmin/{id}")]
        public IActionResult DeleteAdmin([FromRoute] int id)
        {
            var response = _adminService.DeleteAdmin(id);
            return response.Send();
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] DBConnectionFactory.ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "Username is required." });
            }

            try
            {
                var admin = _adminService.GetAdminByUsername(request.Username);
                if (admin == null)
                {
                    return NotFound(new { message = "Admin not found." });
                }

                string subject = "Forgot Password - Medpharm";
                string plainTextContent =
                    $"Hello {admin.FullName},\n\nYour password is: {admin.Password}\n\nIf you didn't request this, please contact support.\n\nRegards,\nMedpharm Team";
                string htmlContent =
                    $"<p>Hello {admin.FullName},</p><p>Your password is: <strong>{admin.Password}</strong></p><p>If you didn't request this, please contact support.</p><p>Regards,<br>Medpharm Team</p>";

                await _emailService.SendEmailAsync(admin.Email, subject, plainTextContent, htmlContent);
                return Ok(new { message = "Password has been sent to the registered email." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return StatusCode(500, new { message = "Error processing request.", error = ex.Message });
            }
        }
    }

    // Login request model
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
