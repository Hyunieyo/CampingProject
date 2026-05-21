using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Data
{
    public class AppDbContext : DbContext
    {
        // 1. 생성자 설정: appsettings.json에 적은 DB 주소(Connection String)를 EF Core가 읽을 수 있게 넘겨줍니다.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 2. DB 테이블 등록: 여기에 적은 이름들이 실제 SQL Server에 생성될 테이블 이름이 됩니다!
        public DbSet<Camp> Camps { get; set; } = null;          // 캠핑장 정보 테이블
        public DbSet<User> Users { get; set; } = null;          // 회원 정보 테이블
        public DbSet<Favorite> Favorites { get; set; } = null;  // 즐겨찾기 테이블
        public DbSet<SearchHistory> SearchHistories { get; set; } = null;   // 최근 검색 기록
        public DbSet<CampItem> CampItems { get; set; } = null;  // 캠핑 장보기 테이블

        // 3. (선택) 테이블 규칙 추가 정의
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CampItem>().HasData(
        // ==========================================
        // 1. 주거 및 침구 (인원수 비례 또는 팀당 기본 제공)
        // ==========================================
        new CampItem { Id = 1, Category = "주거/침구", Name = "텐트 본체 및 플라이", Unit = "동", PerAdultAmount = 0.3 }, // 약 3명당 텐트 1동 기준
        new CampItem { Id = 2, Category = "주거/침구", Name = "폴대, 팩, 망치 세트", Unit = "세트", PerAdultAmount = 0.3 },
        new CampItem { Id = 3, Category = "주거/침구", Name = "그라운드시트 (방수포)", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 4, Category = "주거/침구", Name = "타프 (그늘막)", Unit = "동", PerAdultAmount = 0.2 },     // 4~5명당 1개
        new CampItem { Id = 5, Category = "주거/침구", Name = "캠핑 매트 (발포/에어)", Unit = "개", PerAdultAmount = 1.0 }, // 1인당 1개
        new CampItem { Id = 6, Category = "주거/침구", Name = "침낭", Unit = "개", PerAdultAmount = 1.0 },                 // 1인당 1개
        new CampItem { Id = 7, Category = "주거/침구", Name = "캠핑용 베개", Unit = "개", PerAdultAmount = 1.0 },             // 1인당 1개
        new CampItem { Id = 8, Category = "주거/침구", Name = "전기매트", Unit = "개", PerAdultAmount = 0.5 },                 // 2인당 1개
        new CampItem { Id = 9, Category = "주거/침구", Name = "캠핑 의자", Unit = "개", PerAdultAmount = 1.0 },                // 1인당 1개
        new CampItem { Id = 10, Category = "주거/침구", Name = "캠핑 테이블", Unit = "개", PerAdultAmount = 0.3 },             // 3~4인당 1개

        // ==========================================
        // 2. 조명 및 전기
        // ==========================================
        new CampItem { Id = 11, Category = "조명/전기", Name = "메인 랜턴 (고광량)", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 12, Category = "조명/전기", Name = "이너 랜턴 (텐트 내부용)", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 13, Category = "조명/전기", Name = "감성 무드 랜턴", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 14, Category = "조명/전기", Name = "헤드 랜턴 / 플래시", Unit = "개", PerAdultAmount = 0.5 },      // 2인당 1개 꼴
        new CampItem { Id = 15, Category = "조명/전기", Name = "릴선/작업선 (20m)", Unit = "개", PerAdultAmount = 0.2 },        // 팀당 1개 기본
        new CampItem { Id = 16, Category = "조명/전기", Name = "멀티탭", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 17, Category = "조명/전기", Name = "고용량 보조 배터리", Unit = "개", PerAdultAmount = 0.5 },

        // ==========================================
        // 3. 취사 및 조리도구
        // ==========================================
        new CampItem { Id = 18, Category = "취사도구", Name = "휴대용 가스버너 (구이바다)", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 19, Category = "취사도구", Name = "부탄/이소가스", Unit = "개", PerAdultAmount = 1.0 },            // 1인당 1개꼴 여유분
        new CampItem { Id = 20, Category = "취사도구", Name = "바베큐 화로대 & 그릴", Unit = "개", PerAdultAmount = 0.2 },
        new CampItem { Id = 21, Category = "취사도구", Name = "참숯 / 장작", Unit = "박스", PerAdultAmount = 0.5 },           // 2인당 1박스
        new CampItem { Id = 22, Category = "취사도구", Name = "캠핑 토치 (착화제 포함)", Unit = "개", PerAdultAmount = 0.2 },
        new CampItem { Id = 23, Category = "취사도구", Name = "코펠 세트 (냄비/팬)", Unit = "세트", PerAdultAmount = 0.3 },
        new CampItem { Id = 24, Category = "취사도구", Name = "칼, 가위, 집게 세트", Unit = "세트", PerAdultAmount = 0.3 },
        new CampItem { Id = 25, Category = "취사도구", Name = "국자, 뒤집개, 도마", Unit = "세트", PerAdultAmount = 0.2 },
        new CampItem { Id = 26, Category = "취사도구", Name = "개인 식기 (밥/국그릇)", Unit = "세트", PerAdultAmount = 1.0 },   // 1인당 1개
        new CampItem { Id = 27, Category = "취사도구", Name = "수저 세트", Unit = "세트", PerAdultAmount = 1.0 },               // 1인당 1개
        new CampItem { Id = 28, Category = "취사도구", Name = "시에라 컵 / 텀블러", Unit = "개", PerAdultAmount = 1.0 },         // 1인당 1개
        new CampItem { Id = 29, Category = "취사도구", Name = "아이스박스 / 쿨러", Unit = "개", PerAdultAmount = 0.3 },
        new CampItem { Id = 30, Category = "취사도구", Name = "주방세제 및 수세미", Unit = "세트", PerAdultAmount = 0.2 },
        new CampItem { Id = 31, Category = "취사도구", Name = "호일 / 키친타월", Unit = "세트", PerAdultAmount = 0.2 },

        // ==========================================
        // 4. 식재료 파트 (★인원수 연산의 핵심)
        // ==========================================
        new CampItem { Id = 32, Category = "식재료", Name = "삼겹살", Unit = "g", PerAdultAmount = 200.0 },               // 1인당 200g
        new CampItem { Id = 33, Category = "식재료", Name = "목살", Unit = "g", PerAdultAmount = 150.0 },                 // 1인당 150g
        new CampItem { Id = 34, Category = "식재료", Name = "가브리살/항정살", Unit = "g", PerAdultAmount = 100.0 },       // 1인당 100g
        new CampItem { Id = 35, Category = "식재료", Name = "살치살 (소고기)", Unit = "g", PerAdultAmount = 100.0 },       // 1인당 100g
        new CampItem { Id = 36, Category = "식재료", Name = "모듬 쌈야채 & 파채", Unit = "팩", PerAdultAmount = 0.3 },       // 3인당 1팩
        new CampItem { Id = 37, Category = "식재료", Name = "구이용 소시지 / 치즈", Unit = "팩", PerAdultAmount = 0.3 },
        new CampItem { Id = 38, Category = "식재료", Name = "봉지 / 컵라면", Unit = "개", PerAdultAmount = 1.5 },           // 1인당 1.5개
        new CampItem { Id = 39, Category = "식재료", Name = "햇반 / 즉석밥", Unit = "개", PerAdultAmount = 1.5 },           // 1인당 1.5개
        new CampItem { Id = 40, Category = "식재료", Name = "김치", Unit = "팩", PerAdultAmount = 0.2 },                   // 5인당 1kg팩 대략 기준
        new CampItem { Id = 41, Category = "식재료", Name = "소주 및 맥주", Unit = "병/캔", PerAdultAmount = 3.0 },         // 1인당 평균 3병/캔
        new CampItem { Id = 42, Category = "식재료", Name = "생수 (2L)", Unit = "병", PerAdultAmount = 1.0 },              // 1인당 하루 1병(2L)
        new CampItem { Id = 43, Category = "식재료", Name = "음료수 / 토닉워터", Unit = "페트", PerAdultAmount = 1.0 },

        // ==========================================
        // 5. 위생 및 개인용품
        // ==========================================
        new CampItem { Id = 44, Category = "위생용품", Name = "대용량 물티슈", Unit = "팩", PerAdultAmount = 0.3 },        // 3인당 1팩
        new CampItem { Id = 45, Category = "위생용품", Name = "두루마리 휴지", Unit = "롤", PerAdultAmount = 0.5 },
        new CampItem { Id = 46, Category = "위생용품", Name = "종량제 및 쓰레기봉투", Unit = "장", PerAdultAmount = 0.5 },
        new CampItem { Id = 47, Category = "위생용품", Name = "모기향 / 에프킬라 세트", Unit = "세트", PerAdultAmount = 0.2 },
        new CampItem { Id = 48, Category = "위생용품", Name = "개인 세면도구 & 타월", Unit = "세트", PerAdultAmount = 1.0 }, // 1인당 1개

        // ==========================================
        // 6. 의류 및 비상약
        // ==========================================
        new CampItem { Id = 49, Category = "의류/비상약", Name = "여벌 옷 및 속옷/양말", Unit = "세트", PerAdultAmount = 1.0 },
        new CampItem { Id = 50, Category = "의류/비상약", Name = "방한 겉옷 (플리스/패딩)", Unit = "벌", PerAdultAmount = 1.0 },
        new CampItem { Id = 51, Category = "의류/비상약", Name = "캠핑용 편한 신발 (크록스)", Unit = "켤레", PerAdultAmount = 1.0 },
        new CampItem { Id = 52, Category = "의류/비상약", Name = "구급상자 (해열제/소화제/밴드)", Unit = "세트", PerAdultAmount = 0.2 } // 팀당 1개
    );
        }
    }
}
