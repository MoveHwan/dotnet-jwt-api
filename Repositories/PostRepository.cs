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

        public async Task<PagedResponse<PostResponse>> GetPagedResponseAsync(
             int page,
             int pageSize,
             string? search,
             string sort,
             string? author,
             DateTime? fromDate,
             DateTime? toDate
        )
        {
            var query = _context.Posts.AsQueryable();

            // query = query.Where(...) => 조건은 계속 누적됨
            // Where → OrderBy → Count → Skip/Take → Select
            // 위 순서를 유지해야 성능이 좋다

            // 검색 (제목 + 내용)
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Title.Contains(search) ||
                    p.Content.Contains(search));
            }

            // 작성자 필터
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(p => p.User.Name.Contains(author));
            }

            // 날짜 필터
            if (fromDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(p => p.CreatedAt <= toDate.Value);
            }

            // 정렬
            query = sort switch
            {
                "oldest" => query.OrderBy(p => p.CreatedAt),
                "title" => query.OrderBy(p => p.Title),
                _ => query.OrderByDescending(p => p.CreatedAt) // latest
            };

            var totalCount = await query.CountAsync();

            var items = await query
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