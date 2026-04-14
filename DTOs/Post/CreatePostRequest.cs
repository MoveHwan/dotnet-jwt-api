namespace Test.DTOs.Post
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
