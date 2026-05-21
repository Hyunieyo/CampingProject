using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null)
                return BadRequest(new { message = "로그인 정보가 없습니다." });

            var user = _userService.Login(loginRequest.UserId, loginRequest.Password);

            if (user == null)
                return Unauthorized(new { message = "아이디 또는 비밀번호가 올바르지 않습니다." });

            return Ok(user);
        }
    }
}
