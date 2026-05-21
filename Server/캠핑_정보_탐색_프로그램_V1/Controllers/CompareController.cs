using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [ApiController]
    [Route("api/compare")] // 기본 주소: /api/compare
    public class CompareController : ControllerBase
    {
        private readonly ICompareService _compareService;

        public CompareController(ICompareService compareService)
        {
            _compareService = compareService;
        }

        // POST /api/compare
        [HttpPost]
        public IActionResult CompareCamps([FromBody] List<Camp> campsRequest)
        {
            var result = _compareService.CompareCamps(campsRequest);
            return Ok(result);
        }
    }
}
