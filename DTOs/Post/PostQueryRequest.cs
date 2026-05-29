namespace Test.DTOs.Post
{
    public class PostQueryRequest
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Keyword { get; set; }

        public string SortBy { get; set; } = "latest";
    }
}