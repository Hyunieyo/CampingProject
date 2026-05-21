using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories
{
    public interface IFavoriteRepository
    {
        void Add(Favorite favorite);
        void Delete(int favoriteId);
        List<Favorite> GetByUserId(string userId);
    }
}
