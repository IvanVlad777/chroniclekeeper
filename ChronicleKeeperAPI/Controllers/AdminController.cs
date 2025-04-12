using ChronicleKeeper.Core.Entities.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChronicleKeeperAPI.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    //[Route("api/[controller]")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            Console.WriteLine("GetAllRoles() called! Fetching all users.");
            _logger.LogInformation("Fetching all users");
            var users = _userManager.Users.Select(u => new { u.Id, u.UserName, u.Email }).ToList();
            return Ok(users);
        }

        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            Console.WriteLine("GetAllRoles() called!");
            _logger.LogInformation("Fetching all roles");

            var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Ok(roles);
        }

        [HttpPost("users/assignrole")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound("User not found");

            var roleExists = await _roleManager.RoleExistsAsync(request.Role);
            if (!roleExists) return BadRequest("Role does not exist");

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok($"Role '{request.Role}' assigned to {user.UserName}");
        }

        [HttpPost("users/removerole")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok($"Role '{request.Role}' removed from {user.UserName}");
        }

        [HttpGet("roles/{roleName}/users")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists) return NotFound("Role does not exist.");

            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var userList = users.Select(u => new { u.Id, u.UserName, u.Email }).ToList();

            return Ok(userList);
        }
    }
}
