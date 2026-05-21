using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace 캠핑_정보_탐색_프로그램_V1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PerAdultAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Camps",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    FacltNm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LineIntro = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Intro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoNm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SigunguNm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Addr1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Addr2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MapX = table.Column<double>(type: "float", nullable: false),
                    MapY = table.Column<double>(type: "float", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Homepage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ResveUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FeatureNm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Induty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LctCl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnimalCmgCl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SbrsCl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GlampSiteCo = table.Column<int>(type: "int", nullable: false),
                    CaravSiteCo = table.Column<int>(type: "int", nullable: false),
                    AutoSiteCo = table.Column<int>(type: "int", nullable: false),
                    GnrlSiteCo = table.Column<int>(type: "int", nullable: false),
                    FirstImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camps", x => x.ContentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CampId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK_Favorites_Camps_CampId",
                        column: x => x.CampId,
                        principalTable: "Camps",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    SearchHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SearchedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.SearchHistoryId);
                    table.ForeignKey(
                        name: "FK_SearchHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CampItems",
                columns: new[] { "Id", "Category", "Name", "PerAdultAmount", "Unit" },
                values: new object[,]
                {
                    { 1, "주거/침구", "텐트 본체 및 플라이", 0.29999999999999999, "동" },
                    { 2, "주거/침구", "폴대, 팩, 망치 세트", 0.29999999999999999, "세트" },
                    { 3, "주거/침구", "그라운드시트 (방수포)", 0.29999999999999999, "개" },
                    { 4, "주거/침구", "타프 (그늘막)", 0.20000000000000001, "동" },
                    { 5, "주거/침구", "캠핑 매트 (발포/에어)", 1.0, "개" },
                    { 6, "주거/침구", "침낭", 1.0, "개" },
                    { 7, "주거/침구", "캠핑용 베개", 1.0, "개" },
                    { 8, "주거/침구", "전기매트", 0.5, "개" },
                    { 9, "주거/침구", "캠핑 의자", 1.0, "개" },
                    { 10, "주거/침구", "캠핑 테이블", 0.29999999999999999, "개" },
                    { 11, "조명/전기", "메인 랜턴 (고광량)", 0.29999999999999999, "개" },
                    { 12, "조명/전기", "이너 랜턴 (텐트 내부용)", 0.29999999999999999, "개" },
                    { 13, "조명/전기", "감성 무드 랜턴", 0.29999999999999999, "개" },
                    { 14, "조명/전기", "헤드 랜턴 / 플래시", 0.5, "개" },
                    { 15, "조명/전기", "릴선/작업선 (20m)", 0.20000000000000001, "개" },
                    { 16, "조명/전기", "멀티탭", 0.29999999999999999, "개" },
                    { 17, "조명/전기", "고용량 보조 배터리", 0.5, "개" },
                    { 18, "취사도구", "휴대용 가스버너 (구이바다)", 0.29999999999999999, "개" },
                    { 19, "취사도구", "부탄/이소가스", 1.0, "개" },
                    { 20, "취사도구", "바베큐 화로대 & 그릴", 0.20000000000000001, "개" },
                    { 21, "취사도구", "참숯 / 장작", 0.5, "박스" },
                    { 22, "취사도구", "캠핑 토치 (착화제 포함)", 0.20000000000000001, "개" },
                    { 23, "취사도구", "코펠 세트 (냄비/팬)", 0.29999999999999999, "세트" },
                    { 24, "취사도구", "칼, 가위, 집게 세트", 0.29999999999999999, "세트" },
                    { 25, "취사도구", "국자, 뒤집개, 도마", 0.20000000000000001, "세트" },
                    { 26, "취사도구", "개인 식기 (밥/국그릇)", 1.0, "세트" },
                    { 27, "취사도구", "수저 세트", 1.0, "세트" },
                    { 28, "취사도구", "시에라 컵 / 텀블러", 1.0, "개" },
                    { 29, "취사도구", "아이스박스 / 쿨러", 0.29999999999999999, "개" },
                    { 30, "취사도구", "주방세제 및 수세미", 0.20000000000000001, "세트" },
                    { 31, "취사도구", "호일 / 키친타월", 0.20000000000000001, "세트" },
                    { 32, "식재료", "삼겹살", 200.0, "g" },
                    { 33, "식재료", "목살", 150.0, "g" },
                    { 34, "식재료", "가브리살/항정살", 100.0, "g" },
                    { 35, "식재료", "살치살 (소고기)", 100.0, "g" },
                    { 36, "식재료", "모듬 쌈야채 & 파채", 0.29999999999999999, "팩" },
                    { 37, "식재료", "구이용 소시지 / 치즈", 0.29999999999999999, "팩" },
                    { 38, "식재료", "봉지 / 컵라면", 1.5, "개" },
                    { 39, "식재료", "햇반 / 즉석밥", 1.5, "개" },
                    { 40, "식재료", "김치", 0.20000000000000001, "팩" },
                    { 41, "식재료", "소주 및 맥주", 3.0, "병/캔" },
                    { 42, "식재료", "생수 (2L)", 1.0, "병" },
                    { 43, "식재료", "음료수 / 토닉워터", 1.0, "페트" },
                    { 44, "위생용품", "대용량 물티슈", 0.29999999999999999, "팩" },
                    { 45, "위생용품", "두루마리 휴지", 0.5, "롤" },
                    { 46, "위생용품", "종량제 및 쓰레기봉투", 0.5, "장" },
                    { 47, "위생용품", "모기향 / 에프킬라 세트", 0.20000000000000001, "세트" },
                    { 48, "위생용품", "개인 세면도구 & 타월", 1.0, "세트" },
                    { 49, "의류/비상약", "여벌 옷 및 속옷/양말", 1.0, "세트" },
                    { 50, "의류/비상약", "방한 겉옷 (플리스/패딩)", 1.0, "벌" },
                    { 51, "의류/비상약", "캠핑용 편한 신발 (크록스)", 1.0, "켤레" },
                    { 52, "의류/비상약", "구급상자 (해열제/소화제/밴드)", 0.20000000000000001, "세트" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CampId",
                table: "Favorites",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_UserId",
                table: "SearchHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampItems");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "SearchHistories");

            migrationBuilder.DropTable(
                name: "Camps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
