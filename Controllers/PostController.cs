using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test.DTOs.Auth;
using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.DTOs.User;
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
        public async Task<IActionResult> GetPosts(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string sort = "latest",
            string? author = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
        )
        {
            var response = await _postService.GetPostsAsync(page, pageSize, search, sort, author, fromDate, toDate);

            return Ok(ApiResponse<PagedResponse<PostResponse>>.SuccessResponse(response));
        }

        // 단일 글 조회
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var response = await _postService.GetByIdAsync(id);

            if (response == null)
                return NotFound(ApiResponse<PostResponse>.FailResponse("글 없음"));

            return Ok(ApiResponse<PostResponse>.SuccessResponse(response));
        }

        // 글 생성
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>.FailResponse("인증 정보 없음"));

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.CreateAsync(userId, request);

            return CreatedAtAction(
                nameof(GetPostById), 
                new { id = response.Id }, 
                ApiResponse<PostResponse>.SuccessResponse(response, "글 생성 성공")
            );
        }

        // 글 수정
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, UpdatePostRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>.FailResponse("인증 정보 없음"));

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.UpdateAsync(id, userId, request);

            if (response == null)
                return NotFound(ApiResponse<PostResponse>.FailResponse("글 수정 실패"));

            return Ok(ApiResponse<PostResponse>.SuccessResponse(response));
        }

        // 글 삭제
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>.FailResponse("인증 정보 없음"));

            var userId = int.Parse(userIdClaim.Value);

            var response = await _postService.DeleteAsync(id, userId);

            if (!response)
                return NotFound(ApiResponse<string>.FailResponse("글 삭제 실패"));

            return Ok(ApiResponse<string>.SuccessResponse("삭제 완료"));
        }
    }
}