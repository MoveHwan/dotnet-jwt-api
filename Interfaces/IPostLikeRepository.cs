using Test.Models;

namespace Test.Interfaces
{
    public interface IPostLikeRepository
    {
        Task<PostLike?> GetAsync(int userId, int postId);

        Task<int> CountByPostIdAsync(int postId);

        Task AddAsync(PostLike postLike);

        Task DeleteAsync(PostLike postLike);

        Task SaveChangesAsync();
    }
}