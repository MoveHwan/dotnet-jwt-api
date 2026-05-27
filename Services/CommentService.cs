using AutoMapper;
using Test.DTOs.Comment;
using Test.Interfaces;
using Test.Models;

namespace Test.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CommentService(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IPostRepository postRepository,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        // 게시글 댓글 조회
        public async Task<List<CommentResponse>> GetByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetByPostIdAsync(postId);

            var responses = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                var user = await _userRepository.GetByIdAsync(comment.UserId);

                var response = _mapper.Map<CommentResponse>(comment);

                response.AuthorName = user?.Name ?? "";

                responses.Add(response);
            }

            return responses;
        }

        // 댓글 생성
        public async Task<CommentResponse?> CreateAsync(
            int userId,
            int postId,
            CreateCommentRequest request)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException("게시글 없음");

            var comment = new Comment
            {
                Content = request.Content,
                UserId = userId,
                PostId = postId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);

            var response = _mapper.Map<CommentResponse>(comment);

            response.AuthorName = user?.Name ?? "";

            return response;
        }

        // 댓글 수정
        public async Task<CommentResponse?> UpdateAsync(
            int commentId,
            int userId,
            UpdateCommentRequest request)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
                throw new KeyNotFoundException("댓글 없음");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("수정 권한 없음");

            comment.Content = request.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);

            var response = _mapper.Map<CommentResponse>(comment);

            response.AuthorName = user?.Name ?? "";

            return response;
        }

        // 댓글 삭제
        public async Task<bool> DeleteAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment == null)
                throw new KeyNotFoundException("댓글 없음");

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("삭제 권한 없음");

            await _commentRepository.DeleteAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return true;
        }
    }
}