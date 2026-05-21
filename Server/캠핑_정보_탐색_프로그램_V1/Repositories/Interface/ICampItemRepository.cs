using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Interface
{
    public interface ICampItemRepository
    {
        Task<IEnumerable<CampItem>> GetAllItemsAsync();
    }
}
