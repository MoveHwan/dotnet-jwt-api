using Test.Models;

namespace Test.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetByPostIdAsync(int postId);

        Task<Comment?> GetByIdAsync(int id);

        Task AddAsync(Comment comment);

        Task DeleteAsync(Comment comment);

        Task SaveChangesAsync();
    }
}