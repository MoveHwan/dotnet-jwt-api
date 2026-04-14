using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Content { get; set; } = string.Empty;

        // 외래키
        public int UserId { get; set; }

        // 네비게이션 속성
        public User? User { get; set; }

        // 생성일
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 수정일
        public DateTime? UpdatedAt { get; set; }
    }
}