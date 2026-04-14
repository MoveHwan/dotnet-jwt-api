using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test.DTOs.Post;
using Test.Services;

namespace Test.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        // 전체 글 조회
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPosts(int page = 1, int pageSize = 10)
        {
            var responses = await _postService.GetPostsAsync(page, pageSize);

            return Ok(responses);
        }

        // 글 생성
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.CreateAsync(userId, request);

            return CreatedAtAction(nameof(GetPosts), new { id = response.Id }, response);
        }

        // 글 수정
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, UpdatePostRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.UpdateAsync(id, userId, request);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        // 글 삭제
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.DeleteAsync(id, userId);

            if (!response)
                return NotFound();

            return Ok("삭제 완료");
        }
    }
}