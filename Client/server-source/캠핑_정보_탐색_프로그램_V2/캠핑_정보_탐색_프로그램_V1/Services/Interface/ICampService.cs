using 캠핑_정보_탐색_프로그램_V1.Models.DTO;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Interface
{
    public interface ICampService
    {
        // 전체 목록 가져오기 (DTO로 변환해서)
        Task<IEnumerable<CampDto>> GetAllCampsAsync();

        // 키워드 검색
        Task<IEnumerable<CampDto>> SearchCampsAsync(string keyword);

        // 지역 필터 (DoNm 기준)
        Task<IEnumerable<CampDto>> GetCampsByFilterAsync(string region);

        // 상세 조회
        Task<CampDto> GetCampByIdAsync(int id);

        // 저장 
        Task SaveCampsAsync(IEnumerable<CampDto> campDtos);
    }
}
