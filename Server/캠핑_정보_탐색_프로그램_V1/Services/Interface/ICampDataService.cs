using 캠핑_정보_탐색_프로그램_V1.Models.DTO;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Interface
{
    public interface ICampDataService
    {
        Task<List<CampDto>> SearchCampAsync(string keyword);
        Task<List<CampDto>> GetAllCampAsync();
    }
}
