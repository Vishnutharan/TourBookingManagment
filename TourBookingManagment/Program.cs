using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TourBookingManagment.Database;

var builder = WebApplication.CreateBuilder(args);

// Add CORS Support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Allow Angular frontend on this port
              .AllowAnyHeader()                     // Allow any header
              .AllowAnyMethod()                     // Allow any HTTP method (GET, POST, etc.)
              .AllowCredentials();                  // Allow credentials (cookies, headers, etc.)
    });
});

// Add Controllers
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tour Booking API",
        Version = "v1",
        Description = "API documentation for Tour Booking Management System"
    });

    // JWT Security Definition for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Entity Framework and Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication Configuration
var key = Encoding.ASCII.GetBytes(
    builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in Development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tour Booking API v1");
        c.RoutePrefix = "swagger"; // Serve Swagger UI at /swagger
    });
}

// Middleware for HTTPS redirection (optional based on your setup)
app.UseHttpsRedirection();

// Use CORS Policy before authentication and authorization
app.UseCors("AllowFrontend");  // Ensure this comes before Authentication & Authorization

// Enable Authentication and Authorization
app.UseAuthentication();  // Ensure JWT authentication is enabled
app.UseAuthorization();   // Enable authorization

// Rewrite Middleware for Angular Frontend (optional, if hosting frontend on the same server)
app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/api") && context.Request.Path != "/")
    {
        context.Request.Path = "/";  // Redirect any non-API path to the frontend
    }
    await next();
});

// Map Controllers to endpoints
app.MapControllers();

// Run the application
app.Run();
