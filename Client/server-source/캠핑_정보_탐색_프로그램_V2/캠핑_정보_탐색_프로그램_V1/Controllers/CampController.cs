using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [Route("api/camps")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampService _campService;

        public CampController(ICampService campService)
        {
            _campService = campService;
        }

        // 1. 전체 캠핑장 조회
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDto>>> GetAllCamps()
        {
            var camps = await _campService.GetAllCampsAsync();
            return Ok(camps);
        }

        // 2. 키워드 검색
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CampDto>>> SearchCamps([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("검색어를 입력해주세요.");

            var results = await _campService.SearchCampsAsync(keyword);
            return Ok(results);
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
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDto>> GetCampById(int id)
        {
            var camp = await _campService.GetCampByIdAsync(id);
            if (camp == null)
                return NotFound("해당 캠핑장을 찾을 수 없습니다.");

            return Ok(camp);
        }
    }
}
