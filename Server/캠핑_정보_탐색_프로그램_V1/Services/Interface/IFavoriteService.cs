using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Interface
{
    public interface IFavoriteService
    {
        void AddFavorite(string userId, int campId);
        void RemoveFavorite(string userId, int campId);
        List<Favorite> GetMyFavorites(string userId);
    }
}