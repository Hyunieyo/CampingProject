using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.API_Services;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Business;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;
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
builder.Services.AddScoped<ISearchHistoryRepository, SearchHistoryRepository>();
builder.Services.AddScoped<ICampItemRepository, CampItemRepository>();


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
builder.Services.AddScoped<ISearchHistoryService, SearchHistoryService>();
builder.Services.AddScoped<ICampItemService, CampItemService>();


// =========================================================================
// 3. 컨트롤러 및 Swagger 설정 단
// =========================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//==
// CORS 정책 추가
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// 이 줄이 있어야 CORS가 적용됨
app.UseCors("AllowAll");


// =========================================================================
// 4. 파이프라인 및 미들웨어 설정 단
// =========================================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Local WPF client uses HTTP to avoid development certificate issues.
app.UseAuthorization();
app.MapControllers();


// =========================================================================
// 5. 🔥 [최종 수정] 서버 실행 시 공공 API 데이터를 긁어와 DB에 채우는 자동 로직
// =========================================================================
_ = Task.Run(async () =>
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var savedCampCount = await dbContext.Camps.CountAsync();

    if (savedCampCount > 20)
        return;

    try
    {
        Console.WriteLine("[초기화] 공공 API 전체 캠핑장 데이터를 백그라운드에서 가져오는 중...");

        var campDataService = scope.ServiceProvider.GetRequiredService<ICampDataService>();
        var campService = scope.ServiceProvider.GetRequiredService<ICampService>();

        var campDtos = await campDataService.GetAllCampAsync();
        await campService.SaveCampsAsync(campDtos);

        Console.WriteLine($"[성공] 총 {campDtos.Count}개의 캠핑장 데이터가 DB에 초기화되었습니다.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[오류] 초기 데이터 로딩 중 실패: {ex.Message}");
    }
});

// 서버 가동 시작!
app.Run();