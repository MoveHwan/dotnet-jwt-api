using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Models;

namespace Test.Interfaces
{
    public interface IPostRepository
    {
        Task<PagedResponse<PostResponse>> GetPagedResponseAsync(int page, int pageSize);
        Task<PostResponse?> GetPostResponseByIdAsync(int id);
        Task<Post?> GetByIdAsync(int id);
        Task AddAsync(Post post);
        Task DeleteAsync(Post post);
        Task SaveChangesAsync();
    }
}