using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using Medpharm.BusinessModels.Models; // ✅ Reference your model here

namespace Medpharm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly string key = "rzp_test_jWPrsJGpszeYSa";
        private readonly string secret = "ht7h4uRGab7tQwvRZmXkeZOv";

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] PaymentRequest req)
        {
            try
            {
                var client = new RazorpayClient(key, secret);

                var receipt = $"rcpt_{Guid.NewGuid().ToString("N").Substring(0, 25)}"; // Max 40 chars

                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", req.Amount }, // Amount in paise (₹500 = 50000)
                    { "currency", "INR" },
                    { "receipt", receipt },
                    { "payment_capture", 1 }
                };

                Razorpay.Api.Order order = client.Order.Create(options);

                return Ok(new
                {
                    orderId = order["id"].ToString(),
                    amount = order["amount"],
                    currency = order["currency"],
                    key = key
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error initiating payment", details = ex.Message });
            }
        }
    }
}