using AutoMapper;
using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Interfaces;
using Test.Models;
using Test.Repositories;

namespace Test.Services
{
    public class PostService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IMapper _mapper;

        public PostService(IUserRepository userRepository, IPostRepository postRepository, IPostLikeRepository  postLikeRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PostResponse>> GetPostsAsync(PostQueryRequest query)
        {
            var posts = await _postRepository.GetPagedAsync(query);

            var totalCount = await _postRepository.CountAsync(query.Keyword);

            var responses = _mapper.Map<List<PostResponse>>(posts);

            foreach (var response in responses)
            {
                response.LikeCount = await _postLikeRepository.CountByPostIdAsync(response.Id);
            }

            return new PagedResponse<PostResponse>
            {
                Items = responses,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    (double)totalCount / query.PageSize)
            };
        }

        public async Task<PostResponse?> GetByIdAsync(int postId, int? userId = null)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                return null;

            var response = _mapper.Map<PostResponse>(post);

            var user = await _userRepository.GetByIdAsync(post.UserId);

            response.AuthorName = user?.Name ?? "";

            foreach (var comment in response.Comments)
            {
                var commentUser = await _userRepository.GetByIdAsync(comment.UserId);

                comment.AuthorName = commentUser?.Name ?? "";
            }

            response.LikeCount = await _postLikeRepository.CountByPostIdAsync(post.Id);

            if (userId.HasValue)
                response.IsLiked = await _postLikeRepository.GetAsync(userId.Value, post.Id) != null;
            

            return response;
        }

        public async Task<PostResponse?> CreateAsync(int userId, CreatePostRequest request)
        {
            var post = new Post
            {
                UserId = userId,
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);

            var response = _mapper.Map<PostResponse>(post);
            response.AuthorName = user?.Name ?? "";

            return response;
        }

        public async Task<PostResponse?> UpdateAsync(int postId, int userId, UpdatePostRequest request)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException("게시글 없음");

            // 핵심: 작성자 체크
            if (post.UserId != userId)
                throw new UnauthorizedAccessException("수정 권한 없음");

            post.Title = request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);

            var response = _mapper.Map<PostResponse>(post);
            response.AuthorName = user?.Name ?? "";

            return response;
        }

        public async Task<bool> DeleteAsync(int postId, int userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException("게시글 없음");

            // 핵심: 작성자 체크
            if (post.UserId != userId)
                throw new UnauthorizedAccessException("삭제 권한 없음");

            await _postRepository.DeleteAsync(post);
            await _postRepository.SaveChangesAsync();

            return true;
        }
    }
}
