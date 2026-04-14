using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.DTOs.Auth;
using Test.DTOs.User;
using Test.Interfaces;
using Test.Models;

namespace Test.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly JwtService _jwtService;

        // DbContext 주입
        public UserService(AppDbContext context, PasswordService passwordService, JwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        // 유저 생성
        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Age = request.Age,
                Email = request.Email,
                Password = _passwordService.Hash(request.Password), // 암호화
                Role = "User" // 기본
            };

            _context.Users.Add(user);

            // 실제 DB에 반영
            await _context.SaveChangesAsync();

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Role = user.Role
            };

            return response;
        }

        // 전체 유저 조회
        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Age = u.Age,
                Email = u.Email,
                Role = u.Role
            }).ToList();
        }

        // 해당 id 유저 조회
        public async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Role = user.Role
            };

            return response;
        }

        // 해당 id 유저 수정
        public async Task<UserResponse?> UpdateAsync(int id, CreateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.Name = request.Name;
            user.Age = request.Age;
            user.Email = request.Email;

            await _context.SaveChangesAsync();

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Age = user.Age,
                Email = user.Email,
                Role = user.Role
            };

            return response;
        }

        // 해당 id 유저 삭제
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        // Dto
        public async Task<List<UserResponse>> GetUsersAsync()
        {
            // DB에서 바로 변환
            return await _context.Users
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Age = u.Age,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToListAsync();


            // 메모리 변환 
            /*var users = await _context.Users
                .Include(u => u.Posts)
                .ToListAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Age = u.Age,
                Posts = u.Posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title
                }).ToList()
            }).ToList();*/
        }

        // 로그인 로직
        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);

            if (user == null || !_passwordService.Verify(request.Password, user.Password))
                return null;

            var accessToken = _jwtService.CreateToken(user);

            var refreshTokenValue = _jwtService.CreateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = _passwordService.Hash(refreshTokenValue),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };

            return response;
        }

        // 로그인 갱신
        public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
        {
            var tokens = await _context.RefreshTokens
                .Include(t => t.User)
                .ToListAsync();

            var token = tokens.FirstOrDefault(t =>
                _passwordService.Verify(refreshToken, t.Token)
            );

            if (token == null || token.IsRevoked)
                return null;

            if (token.ExpiryDate < DateTime.UtcNow)
                return null;

            token.IsRevoked = true;

            var newRefreshTokenValue = _jwtService.CreateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = _passwordService.Hash(newRefreshTokenValue),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = token.UserId
            };

            _context.RefreshTokens.Add(newRefreshToken);

            var newAccessToken = _jwtService.CreateToken(token.User);

            await _context.SaveChangesAsync();

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue
            };
        }







        /*private static List<User> users = new List<User>();

        private static int nextId = 1;

        // 유저 생성
        public User Create(string name, int age)
        {
            var user = new User
            {
                Id = nextId++,
                Name = name,
                Age = age
            };

            users.Add(user);
            return user;
        }

        // 전체 유저 조회
        public List<User> GetAll()
        {
            return users;
        }


        // 해당 id 유저 조회
        public User? GetById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }


        // 해당 id 유저 수정
        public User? Update(int id, string name, int age)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return null;

            user.Name = name;
            user.Age = age;

            return user;
        }

        // 해당 id 유저 삭제
        public bool Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;

            users.Remove(user);
            return true;
        }*/

    }
}
