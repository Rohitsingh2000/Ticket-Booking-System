using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketBooking.DTO;
using TicketBooking.Models;
using TicketBooking.Service;

namespace TicketBooking.Controllers
{
    public class UserController : Controller
    {
     private readonly UserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        public UserController(UserService userService, UserManager<User> userManager, ILogger<UserController> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            bool check = await _userService.RegisterUser(userDto);
            if(!check)
            {
                return BadRequest();
            }
            _logger.LogTrace("User added in database");
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var token = await _userService.LoginUser(loginUserDto);
            if(token == null)
            {
                _logger.LogTrace("User not found in database");
                return Unauthorized();
            }
                
            return Ok(new { Token = token});
        }

        [HttpPut("Update")]

        public async Task<IActionResult> UpdateUserData([FromBody] UpdateUserDto updateUserDto)
        {
            var userId = User.FindFirstValue("user_id");
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogTrace("User not found in database");
                return NotFound("User not found");
            }

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                var removePassword = await _userManager.RemovePasswordAsync(user);
                if (!removePassword.Succeeded)
                {
                    _logger.LogTrace("Failed to remove old password");
                    return BadRequest("Failed to remove old password.");
                }

                var addPassword = await _userManager.AddPasswordAsync(user, updateUserDto.Password);
                if (!addPassword.Succeeded)
                {
                    _logger.LogTrace("Failed to add new password");
                    return BadRequest("Failed to add new password");
                }
            }

                if(!string.IsNullOrEmpty(updateUserDto.UserName))
                    user.UserName = updateUserDto.UserName;

                var updateUserResult = await _userManager.UpdateAsync(user);
                if (!updateUserResult.Succeeded)
            {
                _logger.LogTrace("Api failed to update user details");
                return BadRequest("Failed to update user details");
            }
            _logger.LogTrace("Data updated successfully");
                return Ok("User details updated successfully");
            }
        }
    }