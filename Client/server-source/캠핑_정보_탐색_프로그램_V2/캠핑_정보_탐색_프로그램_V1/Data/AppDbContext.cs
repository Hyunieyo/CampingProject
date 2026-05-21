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
        public DbSet<Camp> Camps { get; set; } = null;         // 캠핑장 정보 테이블
        public DbSet<User> Users { get; set; } = null;          // 회원 정보 테이블
        public DbSet<Favorite> Favorites { get; set; } = null;  // 즐겨찾기 테이블

        // 3. (선택) 테이블 규칙 추가 정의
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 예: Favorite 테이블에서 User나 Camp가 지워질 때 연쇄 삭제(Cascade) 규칙 등을 정할 수 있습니다.
            // 여기서는 기본 설정을 따르므로 비워두어도 무방합니다.
        }
    }
}
