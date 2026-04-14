using Microsoft.AspNetCore.Mvc;
using Test.DTOs;
using Test.Models;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/hello")]
    public class HelloController : ControllerBase
    {
        // 문자열 출력 테스트
        [HttpGet]
        public string GetHello()
        {
            return "Hello ASP.NET Core!";
        }
    }
}
