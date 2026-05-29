using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Interfaces;
using Test.Models;

namespace Test.Repositories
{
    public class PostLikeRepository : IPostLikeRepository
    {
        private readonly AppDbContext _context;

        public PostLikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PostLike?> GetAsync(int userId, int postId)
        {
            return await _context.PostLikes
                .FirstOrDefaultAsync(pl =>
                    pl.UserId == userId &&
                    pl.PostId == postId);
        }

        public async Task<int> CountByPostIdAsync(int postId)
        {
            return await _context.PostLikes
                .CountAsync(pl => pl.PostId == postId);
        }

        public async Task AddAsync(PostLike postLike)
        {
            await _context.PostLikes.AddAsync(postLike);
        }

        public async Task DeleteAsync(PostLike postLike)
        {
            _context.PostLikes.Remove(postLike);

            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}