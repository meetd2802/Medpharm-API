using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ForgotPasswordController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ForgotPasswordController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        // Validate input
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required.");

        // Call Admin API to verify email
        var adminApiUrl = $"http://localhost:5071/api/Admin/check-email?email={email}";
        var response = await _httpClient.GetAsync(adminApiUrl);

        if (!response.IsSuccessStatusCode)
            return NotFound("Email not found in Admin records.");

        // Proceed with password reset logic here
        return Ok("Password reset link has been sent to your email.");
    }
}