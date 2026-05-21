using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. 즐겨찾기 추가 (찜하기)
        public void Add(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
        }

        // 2. 즐겨찾기 삭제 (찜 취소)
        public void Delete(int favoriteId)
        {
            var favorite = _context.Favorites.Find(favoriteId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }
        }

        // 3. 특정 유저의 즐겨찾기 목록 전체 조회
        public List<Favorite> GetByUserId(string userId)
        {
            // .Include(f => f.Camp)를 쓰면 서현님이 채워둔 캠핑장 정보까지 마법처럼 한 번에 묶여서 옵니다!
            return _context.Favorites
                           .Include(f => f.Camp)
                           .Where(f => f.UserId == userId)
                           .ToList();
        }
    }
}
