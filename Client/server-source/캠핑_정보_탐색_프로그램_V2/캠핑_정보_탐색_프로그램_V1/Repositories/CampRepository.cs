using Microsoft.EntityFrameworkCore;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Repositories
{
    public class CampRepository : ICampRepository
    {
        private readonly AppDbContext _context;

        public CampRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Camp>> GetAllCampsAsync()
        {
            return await _context.Camps.ToListAsync();
        }

        public async Task<IEnumerable<Camp>> SearchCampsAsync(string keyword)
        {
            return await _context.Camps
                .Where(c =>
            (c.FacltNm != null && c.FacltNm.Contains(keyword)) ||
            (c.DoNm != null && c.DoNm.Contains(keyword)) ||
            (c.SigunguNm != null && c.SigunguNm.Contains(keyword)) ||
            (c.Addr1 != null && c.Addr1.Contains(keyword)))
        .ToListAsync();
        }

        public async Task<Camp> GetCampByIdAsync(int contentId)
        {
            return await _context.Camps.FirstOrDefaultAsync(c => c.ContentId == contentId);
        }

        public async Task<IEnumerable<Camp>> GetCampsByRegionAsync(string region)
        {
            return await _context.Camps
                .Where(c =>
                    (c.DoNm != null && c.DoNm.Contains(region)) ||
                    (c.SigunguNm != null && c.SigunguNm.Contains(region)) ||
                    (c.Addr1 != null && c.Addr1.Contains(region)))
                .ToListAsync();
        }

        public async Task SaveCampsAsync(IEnumerable<Camp> camps)
        {
            foreach (var camp in camps)
            {
                bool exists = await _context.Camps
                    .AnyAsync(c => c.ContentId == camp.ContentId);

                if (exists == false)
                {
                    _context.Camps.Add(camp);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
