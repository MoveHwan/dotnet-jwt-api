namespace Test.Services
{
    using BCrypt.Net;

    public class PasswordService
    {
        // ⭐ 비밀번호 암호화
        public string Hash(string password)
        {
            return BCrypt.HashPassword(password);
        }

        // ⭐ 비밀번호 검증
        public bool Verify(string password, string hash)
        {
            return BCrypt.Verify(password, hash);
        }
    }
}
