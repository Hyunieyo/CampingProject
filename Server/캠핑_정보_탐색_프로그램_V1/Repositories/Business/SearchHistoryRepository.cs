using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Business
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly AppDbContext _context;

        public SearchHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(string userId, string keyword)
        {
            keyword = keyword.Trim();

            var existing = await _context.SearchHistories
                .FirstOrDefaultAsync(h => h.UserId == userId && h.Keyword == keyword);

            if (existing != null)
            {
                existing.SearchedAt = DateTime.Now;
            }
            else
            {
                var history = new SearchHistory
                {
                    UserId = userId,
                    Keyword = keyword,
                    SearchedAt = DateTime.Now
                };

                _context.SearchHistories.Add(history);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<SearchHistory>> GetRecentByUserIdAsync(string userId)
        {
            return await _context.SearchHistories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.SearchedAt)
                .Take(5)
                .ToListAsync();
        }

        public async Task DeleteAllByUserIdAsync(string userId)
        {
            var histories = await _context.SearchHistories
                .Where(h => h.UserId == userId)
                .ToListAsync();

            _context.SearchHistories.RemoveRange(histories);
            await _context.SaveChangesAsync();
        }

    }
}
