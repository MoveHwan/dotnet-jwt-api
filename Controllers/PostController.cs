using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Services;

namespace Test.Controllers
{
    [ApiController] // [ApiController] 붙어 있으면 ASP.NET Core가 자동 추론도 한다. FromBody, FromQuery...
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly PostLikeService _postLikeService;

        // 생성자
        public PostController(PostService postService, PostLikeService postLikeService)
        {
            _postService = postService;
            _postLikeService = postLikeService;
        }

        // 전체 글 조회
        [Authorize]
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostQueryRequest query) // [FromQuery] : QueryString 자동 바인딩
        {
            var response = await _postService.GetPostsAsync(query);

            return Ok(ApiResponse<PagedResponse<PostResponse>>.SuccessResponse(response));
        }

        // 단일 글 조회
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            int? userId = null;


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
                userId = int.Parse(userIdClaim.Value);
            

            var response = await _postService.GetByIdAsync(id, userId);

            if (response == null)
                return NotFound(ApiResponse<PostResponse>.FailResponse("글 없음"));

            return Ok(ApiResponse<PostResponse>.SuccessResponse(response));
        }

        // 글 생성
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostRequest request)
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
        }// 기본 JSON 요청 => application/json, 파일 업로드 요청 => multipart/form-data

        // 글 수정
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromForm] UpdatePostRequest request)
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

        // 글 좋아요
        [Authorize]
        [HttpPost("{id}/like")]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>.FailResponse("인증 실패"));
            

            var userId = int.Parse(userIdClaim.Value);

            var liked = await _postLikeService.ToggleLikeAsync(userId, id);
            var message = liked ? "좋아요 추가" : "좋아요 취소";

            return Ok(ApiResponse<string>.SuccessResponse(message));
        }
    }
}