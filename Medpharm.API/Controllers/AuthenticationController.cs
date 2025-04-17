using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public AuthenticationController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("send-password")]
    public async Task<IActionResult> SendPassword([FromBody] int adminId)
    {
        // Fetch Admin data from Admin API
        var adminApiUrl = $"http://localhost:5071/api/Admin/get-admin?id={adminId}";
        var response = await _httpClient.GetAsync(adminApiUrl);

        if (!response.IsSuccessStatusCode)
            return NotFound("Admin not found.");

        var adminData = await response.Content.ReadFromJsonAsync<AdminData>();

        if (adminData == null || string.IsNullOrEmpty(adminData.Email) || string.IsNullOrEmpty(adminData.Password))
            return BadRequest("Invalid Admin data.");

        // Send Email using SendGrid
        var emailSent = await SendEmailAsync(adminData.Email, adminData.Password);

        if (!emailSent)
            return StatusCode(500, "Failed to send email.");

        return Ok("Password has been sent to the registered email.");
    }

    private async Task<bool> SendEmailAsync(string email, string password)
    {
        var apiKey = "YOUR_SENDGRID_API_KEY";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("no-reply@medpharm.com", "Medpharm Support");
        var to = new EmailAddress(email);
        var subject = "Your Account Password";
        var content = $"Hello! Your account password is: {password}";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        var response = await client.SendEmailAsync(msg);

        return response.IsSuccessStatusCode;
    }
}

public class AdminData
{
    public string Email { get; set; }
    public string Password { get; set; }
}
