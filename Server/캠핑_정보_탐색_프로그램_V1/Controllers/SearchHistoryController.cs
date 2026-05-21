using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [ApiController]
    [Route("api/search-histories")]
    public class SearchHistoryController : ControllerBase
    {
        private readonly ISearchHistoryService _searchHistoryService;

        public SearchHistoryController(ISearchHistoryService searchHistoryService)
        {
            _searchHistoryService = searchHistoryService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecentSearchHistories(string userId)
        {
            var histories = await _searchHistoryService.GetRecentByUserIdAsync(userId);
            return Ok(histories);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteSearchHistories(string userId)
        {
            await _searchHistoryService.DeleteAllByUserIdAsync(userId);
            return Ok(new { message = "최근 검색 기록이 삭제되었습니다." });
        }

        public class SearchHistoryRequest
        {
            public string UserId { get; set; } = string.Empty;
            public string Keyword { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> AddSearchHistory([FromBody] SearchHistoryRequest request)
        {
            if (request == null)
                return BadRequest("요청 데이터가 없습니다.");

            if (string.IsNullOrWhiteSpace(request.UserId))
                return BadRequest("사용자 아이디가 없습니다.");

            if (string.IsNullOrWhiteSpace(request.Keyword))
                return BadRequest("검색어가 없습니다.");

            await _searchHistoryService.AddAsync(request.UserId, request.Keyword);

            return Ok(new { message = "최근 검색 기록이 저장되었습니다." });
        }
    }
}
