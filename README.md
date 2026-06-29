# ⛺ SH Camping — 캠핑장 정보 탐색 프로그램

공공 API(고캠핑)를 활용한 캠핑장 검색·비교·즐겨찾기 서비스입니다.  
Web 클라이언트 + WPF 데스크탑 클라이언트 + ASP.NET Core REST API 서버로 구성됩니다.

---

## 주요 기능

- 🔍 **캠핑장 검색** — 키워드 및 위치 기반 검색
- 🔥 **인기 100** — 인기 캠핑장 랭킹 조회
- ⭐ **즐겨찾기** — 관심 캠핑장 저장 및 관리
- ⚖️ **캠핑장 비교** — 두 캠핑장 나란히 비교
- 📦 **물품 체크리스트** — 캠핑 준비물 관리
- 🗺️ **지도** — 캠핑장 위치 지도 확인
- 👤 **로그인/회원가입** — 사용자 계정 관리

---

## 기술 스택

| 구분 | 기술 |
|---|---|
| Web Client | HTML, CSS, JavaScript (Vanilla) |
| Desktop Client | WPF (.NET, C#) |
| Server | ASP.NET Core 8, C# |
| Database | SQL Server (EF Core) |
| 외부 API | 한국관광공사 고캠핑 API |

---

## 프로젝트 구조

```
CampingProject/
├── Client/
│   ├── assets/          # 이미지 리소스
│   ├── index.html       # Web 클라이언트 메인
│   ├── styles.css
│   ├── script.js
│   ├── server-source/   # 서버 V2 소스 (구버전 참고용)
│   └── wpf-source/      # WPF 데스크탑 클라이언트
└── Server/
    └── 캠핑_정보_탐색_프로그램_V1/
        ├── Controllers/ # API 엔드포인트
        ├── Services/    # 비즈니스 로직
        ├── Repositories/# 데이터 접근 계층
        ├── Models/      # Entity / DTO
        ├── Migrations/  # EF Core 마이그레이션
        └── API Services/# 고캠핑 외부 API 연동
```

---

## 시작하기

### 사전 준비

- .NET 8 SDK
- SQL Server (LocalDB 또는 SQLEXPRESS)
- [고캠핑 API 키 발급](https://www.data.go.kr)

### 서버 실행

1. `appsettings.json` 설정

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER\\SQLEXPRESS;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "CampApiSettings": {
    "Url": "http://apis.data.go.kr/B551011/GoCamping",
    "Key": "YOUR_GOCAMPING_API_KEY"
  }
}
```

2. 마이그레이션 및 실행

```bash
dotnet ef database update
dotnet run
```

3. 서버가 실행되면 공공 API에서 캠핑장 데이터를 자동으로 DB에 초기화합니다.

### Web 클라이언트 실행

`Client/index.html` 을 브라우저에서 열거나 Live Server로 실행합니다.

---

## API 문서

서버 실행 후 Swagger UI에서 확인할 수 있습니다.

```
http://localhost:{port}/swagger
```
