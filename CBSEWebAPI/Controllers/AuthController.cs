using CBSE.Service.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CBSEWebAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService; 

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CBSE.Entities.LoginRequest model)
        {
            // Validate the user's credentials (this could be from a database)
            var user = _userService.ValidateUser(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            //Create the claims based on the user data
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("SchoolId", user.SchoolId.ToString()),
                new Claim(ClaimTypes.Role, user.Roles.Rolename)
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtSettings:Issuer"],
                    audience: _configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:ExpirationMinutes"])),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    Token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating token: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
