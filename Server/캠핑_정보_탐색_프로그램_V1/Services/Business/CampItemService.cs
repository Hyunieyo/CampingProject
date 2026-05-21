using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class CampItemService : ICampItemService
    {
        private readonly ICampItemRepository _repository;

        public CampItemService(ICampItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CampItemDto>> GetRecommendedChecklistAsync(int headCount)
        {
            var rawItems = await _repository.GetAllItemsAsync();

            // DB 데이터에 인원수를 곱해 최종 추천 수량 계산 로직 돌리기
            return rawItems.Select(item => new CampItemDto
            {
                Id = item.Id,
                Category = item.Category,
                ItemName = item.Name,
                RecommendedQuantity = Math.Ceiling(item.PerAdultAmount * headCount * 10) / 10, // 소수점 1자리 올림 정제
                Unit = item.Unit,
                IsChecked = false // 프론트 UI에서 사용할 기본 체크값
            }).ToList();
        }
    }
}
