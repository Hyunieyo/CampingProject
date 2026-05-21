using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Services.Business;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;
using System.Linq;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [Route("api/camps")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampService _campService;
        private readonly ISearchHistoryService _searchHistoryService;

        public CampController(ICampService campService, ISearchHistoryService searchHistoryService)
        {
            _campService = campService;
            _searchHistoryService = searchHistoryService;
        }

        // 1. 전체 캠핑장 조회
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDto>>> GetAllCamps(
    int pageNo = 1,
    int numOfRows = 21)
        {
            var camps = await _campService.GetAllCampsAsync();

            var totalCount = camps.Count();

            var pagedCamps = camps
                .Skip((pageNo - 1) * numOfRows)
                .Take(numOfRows);

            return Ok(new
            {
                totalCount,
                items = pagedCamps
            });
        }

        // 2. 키워드 검색



        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CampDto>>> SearchCamps(
            [FromQuery] string keyword = "",
            [FromQuery] string region = "",
            [FromQuery] bool petOnly = false,
            [FromQuery] string userId = "",
            int pageNo = 1,
            int numOfRows = 21)
        {
            // 전체 데이터 가져오기

            var results =
                await _campService
                    .GetAllCampsAsync();

            // 검색 기록 저장

            if (!string.IsNullOrWhiteSpace(userId)
                && !string.IsNullOrWhiteSpace(keyword))
            {
                await _searchHistoryService.AddAsync(
                    userId,
                    keyword
                );
            }

            // 키워드 검색

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                results = results.Where(c =>

                    (c.Name != null &&
                     c.Name.Contains(keyword,
                        StringComparison.OrdinalIgnoreCase))

                    ||

                    (c.Address != null &&
                     c.Address.Contains(keyword,
                        StringComparison.OrdinalIgnoreCase))

                    ||

                    (c.FacilityInfo != null &&
                     c.FacilityInfo.Contains(keyword,
                        StringComparison.OrdinalIgnoreCase))
                );
            }

            // 지역 검색

            if (!string.IsNullOrWhiteSpace(region))
            {
                results = results.Where(c =>

                    (c.DoNm != null &&
                     c.DoNm.Contains(region,
                        StringComparison.OrdinalIgnoreCase))

                    ||

                    (c.Address != null &&
                     c.Address.Contains(region,
                        StringComparison.OrdinalIgnoreCase))
                );
            }

            // 반려동물 필터

            if (petOnly)
            {
                results = results
                    .Where(c => c.PetAllowed);
            }

            // 총 개수

            var totalCount =
                results.Count();

            // 페이지네이션

            var pagedResults =

                results
                    .Skip((pageNo - 1) * numOfRows)
                    .Take(numOfRows);

            return Ok(new
            {
                totalCount,
                items = pagedResults
            });
        }


        // 인기 캠핑장 TOP10
        [HttpGet("popular")]
        public async Task<ActionResult<IEnumerable<CampDto>>> GetPopularCamps()
        {
            var camps =
                await _campService
                    .GetAllCampsAsync();

            var popularCamps =

                camps
                    .OrderByDescending(c => c.ViewCount)
                    .Take(100);

            return Ok(popularCamps);
        }

        // 3. 지역 필터 조회
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampDto>>> GetCampsByFilter([FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return BadRequest("지역을 선택해주세요.");

            var results = await _campService.GetCampsByFilterAsync(location);
            return Ok(results);
        }
        // 4. 상세 정보 조회
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CampDto>> GetCampById(int id)
        {
            await _campService.IncreaseViewCountAsync(id);
            var camp = await _campService.GetCampByIdAsync(id);
            if (camp == null)
                return NotFound("해당 캠핑장을 찾을 수 없습니다.");

            return Ok(camp);
        }
    }
}
