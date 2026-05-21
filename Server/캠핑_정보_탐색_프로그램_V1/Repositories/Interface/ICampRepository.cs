using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Interface
{
    public interface ICampRepository
    {
        Task<IEnumerable<Camp>> GetAllCampsAsync();      // 전체 목록
        Task<IEnumerable<Camp>> SearchCampsAsync(string keyword); // 검색
        Task<Camp> GetCampByIdAsync(int contentId);     // 상세 정보

        Task<IEnumerable<Camp>> GetCampsByRegionAsync(string region);

        Task SaveCampsAsync(IEnumerable<Camp> camps);
        Task IncreaseViewCountAsync(int id);
    }
}
