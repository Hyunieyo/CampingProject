using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Interface
{
    public interface ISearchHistoryRepository
    {
        Task AddAsync(string userId, string keyword);
        Task<List<SearchHistory>> GetRecentByUserIdAsync(string userId);
        Task DeleteAllByUserIdAsync(string userId);
    }
}
