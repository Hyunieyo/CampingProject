using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly ISearchHistoryRepository _repository;

        public SearchHistoryService(ISearchHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(string userId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return;

            if (string.IsNullOrWhiteSpace(keyword))
                return;

            await _repository.AddAsync(userId, keyword);
        }

        public async Task<List<SearchHistory>> GetRecentByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<SearchHistory>();

            return await _repository.GetRecentByUserIdAsync(userId);
        }

        public async Task DeleteAllByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return;

            await _repository.DeleteAllByUserIdAsync(userId);
        }
    }
}
