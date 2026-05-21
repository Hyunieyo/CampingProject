using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        // 생성자에서 레포지토리 인터페이스를 받아옵니다. (의존성 주입)
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Login(string UserId, string Password)
        {
            // 비즈니스 로직: 아이디나 비밀번호가 비어서 오면 바로 컷!
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Password))
            {
                return null;
            }

            // 레포지토리에 요청해서 DB에 진짜 회원 정보가 있는지 확인 후 반환
            return _userRepository.GetUserByIdAndPassword(UserId, Password);
        }
    }
}
