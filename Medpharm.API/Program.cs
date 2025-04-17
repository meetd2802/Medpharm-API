using Medpharm.DataAccess;
using Medpharm.DataAccess.DBConnection;
using Medpharm.DataAccess.Repository;
using Medpharm.Services;
using Medpharm.Services.IService;
using Medpharm.Services.Service;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ExceptionConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

Thread.Sleep(5000); // Add this in Program.cs before DB access to give MySQL time
// Register DBConnectionFactory for Dependency Injection
builder.Services.AddSingleton<DBConnectionFactory>();  // Use Singleton for DBConnectionFactory


// Dependency Injection for Repositories and Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IMedicalUpdateRepository, MedicalUpdateRepository>();
builder.Services.AddScoped<IMedicalUpdateService, MedicalUpdateService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();

builder.Services.AddScoped<IHealthTipRepository, HealthTipRepository>();
builder.Services.AddScoped<IHealthTipService, HealthTipService>();

builder.Services.AddScoped<IWaitlistRepository, WaitlistRepository>();
builder.Services.AddScoped<IWaitlistService, WaitlistService>();

builder.Services.AddScoped<IContactFormRepository, ContactFormRepository>();
builder.Services.AddScoped<IContactFormService, ContactFormService>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5062") // Allow your MVC app
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Required for cookies & authentication
        });
});

builder.Services.AddDistributedMemoryCache(); // Required for session management

// Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Admin session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Global Exception Handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        var errorResponse = new
        {
            message = "An unexpected error occurred.",
            error = exceptionHandlerPathFeature?.Error.Message,  // Show actual error
            stackTrace = exceptionHandlerPathFeature?.Error.StackTrace // Show stack trace
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});

app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles(); 
app.UseSession();  // Ensure session is before authentication & authorization
app.UseAuthorization();
app.MapControllers();
app.Run();
