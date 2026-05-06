using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.DTOs.Common;
using Test.DTOs.Post;
using Test.Interfaces;
using Test.Models;

namespace Test.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<PostResponse>> GetPagedResponseAsync(int page, int pageSize)
        {
            var query = _context.Posts.AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    AuthorName = p.User.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();

            return new PagedResponse<PostResponse>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PostResponse?> GetPostResponseByIdAsync(int id)
        {
            return await _context.Posts
                .Where(p => p.Id == id)
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    AuthorName = p.User.Name,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
        }

        public async Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}