using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test.DTOs.Comment;
using Test.DTOs.Common;
using Test.Services;

namespace Test.Controllers
{
    [ApiController]
    [Route("api")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        // 게시글 댓글 조회
        [HttpGet("posts/{postId}/comments")]
        public async Task<IActionResult> GetComments(int postId)
        {
            var response = await _commentService.GetByPostIdAsync(postId);

            return Ok(ApiResponse<List<CommentResponse>>
                .SuccessResponse(response));
        }

        // 댓글 생성
        [Authorize]
        [HttpPost("posts/{postId}/comments")]
        public async Task<IActionResult> CreateComment(
            int postId,
            CreateCommentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>
                    .FailResponse("인증 실패"));

            var userId = int.Parse(userIdClaim.Value);

            var response = await _commentService.CreateAsync(
                userId,
                postId,
                request);

            return Ok(ApiResponse<CommentResponse>
                .SuccessResponse(response!));
        }

        // 댓글 수정
        [Authorize]
        [HttpPut("comments/{id}")]
        public async Task<IActionResult> UpdateComment(
            int id,
            UpdateCommentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>
                    .FailResponse("인증 실패"));

            var userId = int.Parse(userIdClaim.Value);

            var response = await _commentService.UpdateAsync(
                id,
                userId,
                request);

            return Ok(ApiResponse<CommentResponse>
                .SuccessResponse(response!));
        }

        // 댓글 삭제
        [Authorize]
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<string>
                    .FailResponse("인증 실패"));

            var userId = int.Parse(userIdClaim.Value);

            await _commentService.DeleteAsync(id, userId);

            return Ok(ApiResponse<string>
                .SuccessResponse("댓글 삭제 완료"));
        }
    }
}