using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Business
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        // DB 연결 주머니(DbContext)를 서현님에게 주입받아 사용합니다.
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // 주희(UI)님이 입력한 ID/PW와 일치하는 유저가 DB에 있는지 찾는 함수
        public User GetUserByIdAndPassword(string userId, string password)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId && u.Password == password);            
        }
    }
}
