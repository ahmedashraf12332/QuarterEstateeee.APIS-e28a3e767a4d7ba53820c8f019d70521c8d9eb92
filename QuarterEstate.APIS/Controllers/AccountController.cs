using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Services.Contract;
using QuarterEstate.APIS.Errors;

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
           _userService = userService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user =  await _userService.LoginAsync(loginDto);
            if (user is null)return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);

        }
        [HttpPost("Regisiter")]
        public async Task<ActionResult> Register(RegisterDto loginDto)
        {
            var user = await _userService.RegisterAsync(loginDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Regsitrration"));
            return Ok(user);

        }

    }
}
