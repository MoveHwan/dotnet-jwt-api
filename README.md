# JWT Board API

JWT 기반 게시판 API 서버입니다.

<br>

## Deploy

- Render  
  https://dotnet-jwt-api.onrender.com

- Swagger  
  https://dotnet-jwt-api.onrender.com/swagger/index.html

<br>



## Tech Stack

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=flat&logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-512BD4?style=flat)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=flat&logo=jsonwebtokens&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-DD0031?style=flat)
![FluentValidation](https://img.shields.io/badge/FluentValidation-0F6CBD?style=flat)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&logoColor=white)
![Render](https://img.shields.io/badge/Render-46E3B7?style=flat&logo=render&logoColor=black)

<br>

## Features

### User
- 회원가입
- 로그인
- JWT Access Token / Refresh Token
- Role 기반 권한 처리

### Post
- 게시글 CRUD
- 작성자 권한 검증
- 검색 기능
- 정렬 기능
- 페이징 처리

### Comment
- 댓글 CRUD

### Like
- 좋아요
- 좋아요 취소
- 좋아요 개수
- lsLiked

### Common
- Global Exception Middleware
- FluentValidation
- ApiResponse 응답 통일
- AutoMapper

<br>

## API Endpoints

### Auth

```http
POST   /api/users/register   # 회원가입
POST   /api/users/login      # 로그인
POST   /api/users/refresh    # 토큰 재발급
```

### Posts

```http
GET    /api/posts            # 게시글 목록
GET    /api/posts/{id}       # 게시글 상세
POST   /api/posts            # 게시글 생성
PUT    /api/posts/{id}       # 게시글 수정
DELETE /api/posts/{id}       # 게시글 삭제
```

### Comments

```http
POST   /api/comments         # 댓글 생성
PUT    /api/comments/{id}    # 댓글 수정
DELETE /api/comments/{id}    # 댓글 삭제
```

### Likes

```http
POST   /api/posts/{id}/like  # 좋아요 토글
```


<br>

## Project Structure

```plaintext
Controllers/
Services/
Repositories/
Interfaces/
Models/
DTOs/
Validators/
Middleware/
Data/
Mappings/
```

<br>

## Authentication

#### JWT Bearer Authentication 사용

#### Authorization Header

```http
Authorization: Bearer {token}
```

<br>

## API Example

#### POST /api/users/login

#### Request

```json
{
  "name": "test",
  "password": "Test123"
}
```

#### Response

```json
{
  "success": true,
  "data": {
    "accessToken": "JWT_TOKEN",
    "refreshToken": "REFRESH_TOKEN"
  },
  "message": "로그인 성공"
}
```

<br>

## Database

- PostgreSQL
- Entity Framework Core Migration 사용

<br>

## ERD

![ERD](./assets/erd.png)

<br>

## Troubleshooting

#### SQLite → PostgreSQL Migration

#### 문제
- connection string format error
- Migration 적용 오류

#### 해결
- Npgsql Provider 적용
- PostgreSQL Connection String 수정
- EF Core Migration 재생성

<br>

## Future Improvements
- AWS S3 기반 이미지 저장
- Redis 캐싱 적용
- Unit Test 추가
- CQRS 패턴 적용 검토

<br>

## Author

GitHub: https://github.com/MoveHwan
