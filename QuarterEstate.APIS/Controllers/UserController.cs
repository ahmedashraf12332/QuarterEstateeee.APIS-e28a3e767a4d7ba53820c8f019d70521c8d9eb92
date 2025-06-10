using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.Dto.Auth;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Services.Contract;

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // جلب كل المستخدمين
        [Authorize(Roles = "Admin")]
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {


            var (users, count) = await _userService.GetAllUsersAsync(pageIndex, pageSize);

            var result = new
            {
                TotalCount = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Users = users
            };

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {


            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] RegisterDto dto)
        {

            var createdUser = await _userService.AddUserAsync(dto);
            if (createdUser == null) return BadRequest("User could not be created");
            return Ok(createdUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        {

            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return NotFound("User not found or update failed");

            return Ok("User updated successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {


            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found or delete failed");

            return Ok("User deleted successfully");
        }
        [Authorize(Roles = "Admin")] // <-- فقط الأدمن يمكنه استدعاء هذا
        [HttpPost("change-role")]
        public async Task<IActionResult> ChangeUserRole([FromQuery] string userId, [FromQuery] string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
            {
                return BadRequest("User ID and New Role are required.");
            }

            var result = await _userService.ChangeUserRoleAsync(userId, newRole);

            if (result)
            {
                return Ok(new { Message = $"User role changed successfully to {newRole}" });
            }

            return BadRequest("Could not change user role. Please check if the user and role exist.");
        }
        [Authorize(Roles = "Admin")] // <-- فقط الأدمن يمكنه رؤية قائمة الأدمنز الآخرين
        [HttpGet("admins")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAdmins()
        {
            // استدع الدالة الجديدة التي أنشأناها مع تحديد صلاحية "Admin"
            var admins = await _userService.GetUsersInRoleAsync("Admin");

            return Ok(admins);
        }
        [Authorize(Roles = "Admin")] // <-- فقط الأدمن يمكنه رؤية قائمة كل الوكلاء
        [HttpGet("agents")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAgents()
        {
            // ببساطة نستدعي نفس الدالة ولكن مع تمرير "Agent"
            var agents = await _userService.GetUsersInRoleAsync("Agent");

            return Ok(agents);
        }
        [Authorize(Roles = "Admin")] // <-- الأدمن يمكنه رؤية قائمة المستخدمين
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllNormalUsers()
        {
            // نستدعي نفس الدالة مرة أخرى ولكن مع تمرير "User"
            var users = await _userService.GetUsersInRoleAsync("User");

            return Ok(users);
        }
        // File: QuarterEstate.APIS/Controllers/UserController.cs
        [Authorize(Roles = "Admin")]
        [HttpPost("link-user-to-agent")]
        public async Task<IActionResult> LinkUserToAgent([FromQuery] string userId, [FromQuery] int agentId)
        {
            var result = await _userService.LinkUserToAgentAsync(userId, agentId);

            if (result)
            {
                return Ok(new { Message = "User linked to agent successfully." });
            }

            return BadRequest("Failed to link user to agent. Please check if user has 'Agent' role and if agent ID is correct.");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("agent/{agentId}/user")] // <-- مسار مميز وواضح
        public async Task<ActionResult<UserDto>> GetUserForAgent(int agentId)
        {
            var userDto = await _userService.GetUserForAgentAsync(agentId);

            if (userDto == null)
            {
                return NotFound(new { Message = $"No associated user found for agent with ID {agentId}. The agent may not exist or is not linked." });
            }

            return Ok(userDto);
        }

    }
}

