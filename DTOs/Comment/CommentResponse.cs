namespace Test.DTOs.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public int PostId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}