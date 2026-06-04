using AutoMapper;
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
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IMapper _mapper;

        // 생성자
        public PostService(IUserRepository userRepository, IPostRepository postRepository, IPostLikeRepository  postLikeRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _postLikeRepository = postLikeRepository;
            _mapper = mapper;
        }

        // 전체 글 조회 서비스
        public async Task<PagedResponse<PostResponse>> GetPostsAsync(PostQueryRequest query)
        {
            var posts = await _postRepository.GetPagedAsync(query);

            var totalCount = await _postRepository.CountAsync(query.Keyword);

            var responses = _mapper.Map<List<PostResponse>>(posts);

            // 좋아요 수
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

        // 단일 글 조회 서비스
        public async Task<PostResponse?> GetByIdAsync(int postId, int? userId = null)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                return null;

            var response = _mapper.Map<PostResponse>(post);

            var user = await _userRepository.GetByIdAsync(post.UserId);

            response.AuthorName = user?.Name ?? "";

            // 댓글 조회
            foreach (var comment in response.Comments)
            {
                var commentUser = await _userRepository.GetByIdAsync(comment.UserId);

                comment.AuthorName = commentUser?.Name ?? "";
            }

            // 좋아요 수
            response.LikeCount = await _postLikeRepository.CountByPostIdAsync(post.Id);
            // 좋아요 체크
            if (userId.HasValue)
                response.IsLiked = await _postLikeRepository.GetAsync(userId.Value, post.Id) != null;
            

            return response;
        }

        // 글 생성 서비스
        public async Task<PostResponse?> CreateAsync(int userId, CreatePostRequest request)
        {
            string? imageUrl = null;

            //게시글 이미지
            if (request.Image != null)
            {
                // Guid.NewGuid() : 랜덤 고유값 생성.
                // Path.GetExtension() : 확장자 추출. ex).png...
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

                // 운영체제마다 경로 구분자가 다를 수 있어서 Path.Combine 사용. ex) Window "\", Linux "/"...
                var uploadFolder = Path.Combine("wwwroot", "uploads");

                // 폴더 없으면 생성
                Directory.CreateDirectory(uploadFolder);

                var uploadPath = Path.Combine(uploadFolder, fileName);

                // using var : 작업이 끝나면 자동으로 Dispose() 실행하여 파일 리소스 정리
                using var stream = new FileStream(uploadPath, FileMode.Create);

                // 실제 디스크에 파일 저장
                await request.Image.CopyToAsync(stream);

                imageUrl = $"/uploads/{fileName}";
            }

            var post = new Post
            {
                UserId = userId,
                Title = request.Title,
                Content = request.Content,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(userId);

            var response = _mapper.Map<PostResponse>(post);
            response.AuthorName = user?.Name ?? "";

            return response;
        }

        // 글 수정 서비스
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

        // 글 삭제 서비스
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
