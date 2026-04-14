using Test.Models;

namespace Test.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPagedAsync(int page, int pageSize);
        Task<Post?> GetByIdAsync(int id);
        Task AddAsync(Post post);
        Task DeleteAsync(Post post);
        Task SaveChangesAsync();
    }
}