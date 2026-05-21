using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public void AddFavorite(string userId, int campId)
        {
            var favorite = new Favorite
            {
                UserId = userId,
                CampId = campId
            };
            _favoriteRepository.Add(favorite);
        }

        public void RemoveFavorite(int favoriteId)
        {
            _favoriteRepository.Delete(favoriteId);
        }

        public List<Favorite> GetMyFavorites(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<Favorite>();

            return _favoriteRepository.GetByUserId(userId);
        }
    }
}
