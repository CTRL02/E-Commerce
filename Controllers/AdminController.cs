using E_Commerce.Context;
using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Controllers

{ 
    [ApiController]
    [Route("[Controller]")]
    public class AdminController : ControllerBase
    {

        private readonly ECommerceDbContext _Context;
        private readonly TokenService _TokenService;
        private readonly IEmailSender _EmailSender;
        private JwtOptions _jwtOptions;

        public AdminController(ECommerceDbContext Context, TokenService tokenService, IEmailSender emailSender, JwtOptions jwtOptions1)
        {
            _Context = Context;
            _TokenService = tokenService;
            _EmailSender = emailSender;
            _jwtOptions = jwtOptions1;
        }

        //    [HttpPost("Generate-Password-Hash")]
        //    [AllowAnonymous]
        //    public IActionResult GeneratePasswordHash(LoginDTO loginDTO) {

        //        var admin = new Admin
        //        {
        //            UserName = loginDTO.EmailOrUserName
        //        };
        //        var passwordHasher = new PasswordHasher<Admin>();
        //        admin.Password = passwordHasher.HashPassword(admin, loginDTO.Password);

        //        _Context.Admins.Add(admin);
        //        _Context.SaveChanges();

        //        return Ok("oh yeah sexy");

        //    }

        [AllowAnonymous]
        [HttpPost("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
        public async Task<IActionResult> SigninAdmin([FromBody] LoginDTO login)
        {
            if (string.IsNullOrEmpty(login.EmailOrUserName) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Find Admin by username 
            var admin = await _Context.Admins
                .SingleOrDefaultAsync(s => s.UserName == login.EmailOrUserName);

            if (admin == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Verify password
            var passwordHasher = new PasswordHasher<Admin>();
            var result = passwordHasher.VerifyHashedPassword(admin, admin.Password, login.Password);

            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate token
            var token = TokenGenerator(admin, "Admin");

            return Ok(new
            {
                message = "Sign-in successful!",
                token = token
            });
        }

        private string TokenGenerator(Admin admin, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, admin.UserName),
                    new(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);
            return accessToken;
        }


    }
}
