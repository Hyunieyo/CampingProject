using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.API_Services;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Repositories;
using 캠핑_정보_탐색_프로그램_V1.Services.Business;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;
using 캠핑_정보_탐색_프로그램_V1.Services.Utility;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. 데이터베이스 및 리포지토리(Repository) 등록 단
// =========================================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🌟 [중요] 누락되었던 CampRepository를 서버 부품으로 등록해 줍니다!
builder.Services.AddScoped<ICampRepository, CampRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();


// =========================================================================
// 2. 비즈니스 서비스(Service) 및 API/유틸리티 등록 단
// =========================================================================
builder.Services.AddSingleton<CampApiService>();
builder.Services.AddSingleton<CampJsonParser>();
builder.Services.AddSingleton<DataCleaner>(); // 승빈님의 데이터 정제 도구

builder.Services.AddScoped<ICampService, CampService>();
builder.Services.AddScoped<ICampDataService, CampDataService>(); // 승빈님의 API 검색 서비스
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<ICompareService, CompareService>();


// =========================================================================
// 3. 컨트롤러 및 Swagger 설정 단
// =========================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("WebClient", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


// =========================================================================
// 4. 파이프라인 및 미들웨어 설정 단
// =========================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Local WPF client uses HTTP to avoid development certificate issues.
app.UseCors("WebClient");
app.UseAuthorization();
app.MapControllers();


// =========================================================================
// 5. 🔥 [최종 수정] 서버 실행 시 공공 API 데이터를 긁어와 DB에 채우는 자동 로직
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var campDataService = scope.ServiceProvider.GetRequiredService<ICampDataService>();
    var campService = scope.ServiceProvider.GetRequiredService<ICampService>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // DB 테이블이 아예 비어있을 때만 최초 1회 자동으로 긁어옵니다.
    if (!dbContext.Camps.Any())
    {
        try
        {
            Console.WriteLine("[초기화] 공공 API로부터 캠핑장 데이터를 가져오는 중...");

            // 1. 승빈님이 만든 서비스로 '캠핑' 키워드 데이터를 깨끗하게 긁어옵니다.
            var campDtos = campDataService.SearchCampAsync("캠핑").GetAwaiter().GetResult();

            // 2. 서현님이 업데이트한 새 서비스 기능으로 DB에 일괄 저장합니다.
            campService.SaveCampsAsync(campDtos).GetAwaiter().GetResult();

            Console.WriteLine($"[성공] 총 {campDtos.Count}개의 캠핑장 데이터가 DB에 초기화되었습니다.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[오류] 초기 데이터 로딩 중 실패: {ex.Message}");
        }
    }
}

// 서버 가동 시작!
app.Run();
