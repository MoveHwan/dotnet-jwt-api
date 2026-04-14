using Test.DTOs.User;
using Test.DTOs.Auth;
using Test.Models;

namespace Test.Interfaces
{
    // 서비스의 "설계도"
    // 어떤 기능을 제공할지 정의만 함 (구현 X)
    public interface IUserService
    {
        // 비동기
        Task<List<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
        Task<UserResponse> CreateAsync(CreateUserRequest request);
        Task<UserResponse?> UpdateAsync(int id, CreateUserRequest request);
        Task<bool> DeleteAsync(int id);

        Task <TokenResponse?> LoginAsync(LoginRequest request);
        Task<TokenResponse?> RefreshTokenAsync(string refreshToken);

        /*List<User> GetAll();
        User Create(string name, int age);
        User? GetById(int id);
        User? Update(int id, string name, int age);
        bool Delete(int id);*/

    }
}
