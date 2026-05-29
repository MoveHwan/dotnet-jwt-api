using Test.DTOs.Comment;

namespace Test.DTOs.Post
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }

        public string? ImageUrl { get; set; }

        public List<CommentResponse> Comments { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
