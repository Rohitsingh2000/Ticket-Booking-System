using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketBooking.Service
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);


            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userDto.Role);
            }
            else
            {
                Console.WriteLine("User not registred");
                return false;
            }
                    

            _context.SaveChanges();
            return true;
        }

        public async Task<string> LoginUser(LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
            {
                var token = GenerateJwtToken(user);
                return token;
            }

            return null;
        }

        private string GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("user_id", user.Id),
                new Claim("nbf", ConvertDateTime(DateTime.Now).ToString(), ClaimValueTypes.Integer64),
                new Claim("iat", ConvertDateTime(DateTime.Now).ToString(), ClaimValueTypes.Integer64)
            };

            var userRoles = _userManager.GetRolesAsync(user).Result;
            authClaims.Add(new Claim("role", userRoles.First()));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static long ConvertDateTime(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }
    }
}
