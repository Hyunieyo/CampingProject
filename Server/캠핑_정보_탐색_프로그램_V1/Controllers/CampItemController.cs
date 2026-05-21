using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampItemController : ControllerBase
    {
        private readonly ICampItemService _campitemService;

        // Service 주입받기
        public CampItemController(ICampItemService campitemService)
        {
            _campitemService = campitemService;
        }

        [HttpGet("recommend")]
        public async Task<IActionResult> GetRecommendedChecklist([FromQuery] int headCount)
        {
            if (headCount <= 0)
            {
                return BadRequest("인원수는 1명 이상이어야 합니다.");
            }

            var result = await _campitemService.GetRecommendedChecklistAsync(headCount);
            return Ok(result);
        }
    }
}
