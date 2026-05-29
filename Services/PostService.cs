using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Interfaces;
using Test.Models;

namespace Test.Services
{
    public class PostService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IUserRepository userRepository, IPostRepository postRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PostResponse>> GetPostsAsync(PostQueryRequest query)
        {
            var posts = await _postRepository.GetPagedAsync(query);

            var totalCount = await _postRepository
                .CountAsync(query.Keyword);

            var responses = _mapper
                .Map<List<PostResponse>>(posts);

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

        public async Task<PostResponse?> GetByIdAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            return _mapper.Map<PostResponse>(post);
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
