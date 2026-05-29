using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Models;

namespace Test.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPagedAsync(PostQueryRequest query);
        Task<int> CountAsync(string? keyword);
        Task<Post?> GetByIdAsync(int id);
        Task AddAsync(Post post);
        Task DeleteAsync(Post post);
        Task SaveChangesAsync();
    }
}