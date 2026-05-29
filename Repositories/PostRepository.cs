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

        public async Task<List<Post>> GetPagedAsync(PostQueryRequest query)
        {
            // AsQueryable() : 동적으로 조건 추가 가능
            var postsQuery = _context.Posts.AsQueryable();

            // 검색
            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                postsQuery = postsQuery.Where(p =>
                    p.Title.Contains(query.Keyword));
            }

            // 정렬
            postsQuery = query.SortBy.ToLower() switch
            {
                "oldest" => postsQuery.OrderBy(p => p.CreatedAt),
                _ => postsQuery.OrderByDescending(p => p.CreatedAt)
            };

            // 페이징
            return await postsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string? keyword)
        {
            var query = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p =>
                    p.Title.Contains(keyword));
            }

            return await query.CountAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
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