using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 캠핑_정보_탐색_프로그램_V1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Camps",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camps", x => x.ContentId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CampId",
                table: "Favorites",
                column: "CampId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Camps");
        }
    }
}
