using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Interface
{
    public interface IUserService
    {
        User Login(string UserId, string Password);
    }
}
