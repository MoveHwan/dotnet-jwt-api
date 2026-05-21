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

<br>

## Project Structure

```plaintext
Controllers/
Services/
Repositories/
DTOs/
Middleware/
Models/
Data/
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

## Troubleshooting

#### SQLite → PostgreSQL Migration

#### 문제
- connection string format error 발생
- Render 배포 환경에서 SQLite 영속성 문제 발생

#### 해결
- PostgreSQL로 마이그레이션
- Npgsql Provider 적용
- Render PostgreSQL 사용

<br>

## Author

GitHub: https://github.com/MoveHwan
