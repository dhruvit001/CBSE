using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CBSE.Data.Repositories.Abstractions;
using CBSE.Data.Repositories.Implementations;
using CBSE.Data;
using CBSE.Service.Services.Abstractions;
using CBSE.Service.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Enable logging for debugging purposes
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            // Log the exception when authentication fails
            if (context.Exception != null)
            {
                var errorMessage = context.Exception.Message;
                // You can log the error message to a logging framework like Serilog, NLog, etc.
                Console.WriteLine($"Authentication failed: {errorMessage}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            // Optionally log token validation success
            var claimsPrincipal = context.Principal;
            Console.WriteLine($"Token validated for: {claimsPrincipal?.Identity?.Name}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // Log challenge error if token is missing or invalid
            Console.WriteLine("Token challenge failed");
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // Ensure the issuer and audience match the expected values
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],

        // Ensure the secret key used for signing matches the expected one
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),

        // Handle expired tokens more strictly (optional)
        ClockSkew = TimeSpan.Zero,  // Optional: Set to 0 to eliminate the default 5-minute clock skew
    };

    // Optional: Enable token replay protection (for anti-replay attacks)
    options.SaveToken = true;
});


// Add DbContext with SQL Server connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your repositories and services
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add security definition for Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });

    // Add global security requirement
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
            new string[] { }
        }
    });
});

// Enable CORS (Optional, if you plan to expose this API to frontend apps hosted elsewhere)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    // Use Swagger only in development for API docs
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management API v1"));
}

// Middleware to enable CORS
app.UseCors("AllowAll");

// Use routing and authorization for API endpoints
app.UseHttpsRedirection();

// Use JWT authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
