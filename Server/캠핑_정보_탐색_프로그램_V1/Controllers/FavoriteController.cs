using Microsoft.AspNetCore.Mvc;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Controllers
{
    [ApiController]
    [Route("api/favorites")] // 기본 주소: /api/favorites
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // 1. 즐겨찾기 추가 (POST /api/favorites)
        [HttpPost]
        public IActionResult AddFavorite([FromBody] Favorite favoriteRequest)
        {
            _favoriteService.AddFavorite(favoriteRequest.UserId, favoriteRequest.CampId);
            return Ok(new { message = "즐겨찾기에 추가되었습니다." });
        }

        // 2. 즐겨찾기 삭제 (DELETE /api/favorites/{id})
        [HttpDelete]
        public IActionResult RemoveFavorite([FromQuery] string userId, [FromQuery] int campId)
        {
            _favoriteService.RemoveFavorite(userId, campId);

            return Ok();
        }

        // 3. 내 즐겨찾기 목록 조회 (GET /api/favorites/{userId})
        [HttpGet("{userId}")]
        public IActionResult GetMyFavorites(string userId)
        {
            var favorites = _favoriteService.GetMyFavorites(userId);
            return Ok(favorites);
        }
    }
}
