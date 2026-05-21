using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories.Business
{
    public class CampItemRepository : ICampItemRepository
    {
        private readonly AppDbContext _context;

        public CampItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampItem>> GetAllItemsAsync()
        {
            return await _context.CampItems.ToListAsync();
        }
    }
}
