using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TourBookingManagment.Database;
using TourBookingManagment.Services;
using Stripe;
using Newtonsoft.Json;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UserService>();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://localhost:5173",
                "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tour Booking API",
        Version = "v1",
        Description = "API documentation for Tour Booking Management System with Payment Integration"
    });

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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddMemoryCache();
builder.Services.AddScoped<IStripeService, StripeService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddHttpClient();

var jwtKey = Encoding.ASCII.GetBytes(
    builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
        options.TokenValidationParameters = new TokenValidationParameters
         {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        NameClaimType = ClaimTypes.NameIdentifier // Force this mapping
        };

    // Keep existing events configuration
// Rest of events configuration
options.Events = new JwtBearerEvents
       {
           OnAuthenticationFailed = context =>
           {
               Console.WriteLine($"Authentication failed: {context.Exception.Message}");
               return Task.CompletedTask;
           },
           OnChallenge = context =>
           {
               if (!context.Response.HasStarted)
               {
                   context.Response.StatusCode = 401;
                   context.Response.ContentType = "application/json";
                   var result = JsonConvert.SerializeObject(new { error = "You are not authorized" });
                   return context.Response.WriteAsync(result);
               }
               return Task.CompletedTask;
           },
       };
   });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tour Booking API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/api") && context.Request.Path != "/")
    {
        context.Request.Path = "/";
    }
    await next();
});

app.MapControllers();

app.Run();