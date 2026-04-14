using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.DTOs.Auth;
using Test.DTOs.User;
using Test.Interfaces;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        // 서비스 주입 (Dependency Injection)
        private readonly IUserService _userService;

        // 생성자에서 자동으로 주입됨
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // 비동기
        // 유저 생성
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
        {
            var response = await _userService.CreateAsync(request);
            return Ok(response);
        }


        // 전체 유저 조회
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var response = await _userService.GetAllAsync(); // 로직은 서비스가 처리
            return Ok(response);
        }

        // 해당 id 유저 조회
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var response = await _userService.GetByIdAsync(id);
            if (response == null) return NotFound();

            return Ok(response);
        }

        // 해당 id 유저 수정
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, CreateUserRequest request)
        {
            var response = await _userService.UpdateAsync(id, request);
            if (response == null) return NotFound();

            return Ok(response);
        }

        // 해당 id 유저 삭제
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var response = await _userService.DeleteAsync(id);
            if (!response) return NotFound();

            return Ok("Delete Complete");

        }

        // 로그인
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _userService.LoginAsync(request);

            if (response == null)
                return Unauthorized("아이디 또는 비밀번호 틀림");

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRequest request)
        {
            var response = await _userService.RefreshTokenAsync(request.RefreshToken);

            if (response == null)
                return Unauthorized();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("관리자만 접근 가능");
        }


        /* [Authorize]
         [HttpGet("check")]
         public IActionResult Check()
         {
             var user = HttpContext.User;

             foreach (var claim in user.Claims)
             {
                 Console.WriteLine($"{claim.Type} : {claim.Value}");
             }

             var identity = HttpContext.User.Identity;

             Console.WriteLine(identity.IsAuthenticated);

             return Ok(new
             {
                 IsAuth = User.Identity?.IsAuthenticated,
                 Name = User.Identity?.Name
             });
         }*/

        /*// 유저 생성
        [HttpPost]
        public IActionResult CreateUser(CreateUserRequest request)
        {
            var user = _userService.Create(request.Name, request.Age);
            return Ok(user);
        }

        // 전체 유저 조회
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll(); // 로직은 서비스가 처리
            return Ok(users);
        }

        // 해당 id 유저 조회
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // 해당 id 유저 수정
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, CreateUserRequest request)
        {
            var user = _userService.Update(id, request.Name, request.Age);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // 해당 id 유저 삭제
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _userService.Delete(id);
            if (!result) return NotFound();

            return Ok("Delete Complete");

        }*/

    }
}
