using Test.Interfaces;
using Test.Models;

namespace Test.Services
{
    public class PostLikeService
    {
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IPostRepository _postRepository;

        public PostLikeService(
            IPostLikeRepository postLikeRepository,
            IPostRepository postRepository)
        {
            _postLikeRepository = postLikeRepository;
            _postRepository = postRepository;
        }

        public async Task<bool> ToggleLikeAsync(int userId,int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);

            if (post == null)
                throw new KeyNotFoundException("게시글 없음");

            var existingLike = await _postLikeRepository.GetAsync(userId, postId);

            // 좋아요 이미 존재 → 취소
            if (existingLike != null)
            {
                await _postLikeRepository.DeleteAsync(existingLike);

                await _postLikeRepository.SaveChangesAsync();

                return false;
            }

            // 좋아요 추가
            var postLike = new PostLike
            {
                UserId = userId,
                PostId = postId
            };

            await _postLikeRepository.AddAsync(postLike);

            await _postLikeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetLikeCountAsync(int postId)
        {
            return await _postLikeRepository
                .CountByPostIdAsync(postId);
        }

        public async Task<bool> IsLikedAsync(
            int userId,
            int postId)
        {
            var like = await _postLikeRepository
                .GetAsync(userId, postId);

            return like != null;
        }
    }
}