namespace Test.DTOs.User
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
