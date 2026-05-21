using System.Linq;
using System.Threading.Tasks;
using 캠핑_정보_탐색_프로그램_V1.API_Services;
using 캠핑_정보_탐색_프로그램_V1.Data;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Repositories.Interface;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;


namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class CampService : ICampService
    {
        private readonly ICampRepository _repository;

        // 생성자에서 Repository를 주입받음
        public CampService(ICampRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CampDto>> GetAllCampsAsync()
        {
            var camps = await _repository.GetAllCampsAsync();
            return camps.Select(c => MapToDto(c));
        }

        public async Task<IEnumerable<CampDto>> SearchCampsAsync(string keyword)
        {
            var camps = await _repository.SearchCampsAsync(keyword);
            return camps.Select(c => MapToDto(c));
        }

        // 🌟 인터페이스 규격에 맞춰 반환 타입을 CampDto로 맞추고 null 허용(?) 제거
        public async Task<CampDto> GetCampByIdAsync(int id)
        {
            var camp = await _repository.GetCampByIdAsync(id);
            // 만약 없으면 빈 DTO나 null을 반환하도록 처리
            return camp != null ? MapToDto(camp) : new CampDto();
        }

        public async Task<IEnumerable<CampDto>> GetCampsByFilterAsync(string region)
        {
            var camps = await _repository.GetCampsByRegionAsync(region);
            return camps.Select(c => MapToDto(c));
        }

        // 🌟 Camping ➔ Camp 엔티티로 수정
        private CampDto MapToDto(Camp camp)
        {
            return new CampDto
            {
                Id = camp.ContentId,
                Name = camp.FacltNm ?? "",
                Address = camp.Addr1 ?? "",
                Tel = camp.Tel ?? "",
                // 🌟 Camp.cs에서 위도/경도는 이미 double이므로 ?? 0 연산자 대신 그대로 대입합니다.
                Latitude = camp.MapY,
                Longitude = camp.MapX,
                FacilityInfo = camp.SbrsCl ?? "",
                ImageUrl = camp.FirstImageUrl ?? "",
                Homepage = camp.Homepage ?? "",
                PetAllowed = !string.IsNullOrEmpty(camp.AnimalCmgCl) && !camp.AnimalCmgCl.Contains("불가") && !camp.AnimalCmgCl.Contains("불가능"),
                ViewCount = camp.ViewCount
            };
        }

        public async Task SaveCampsAsync(IEnumerable<CampDto> campDtos)
        {
            // 🌟 dto ➔ Camp 엔티티로 변환하도록 수정
            var camps = campDtos.Select(dto => new Camp
            {
                ContentId = dto.Id,
                FacltNm = dto.Name,
                Addr1 = dto.Address,
                Tel = dto.Tel,
                MapY = dto.Latitude, // 🌟 double형 매핑
                MapX = dto.Longitude,
                SbrsCl = dto.FacilityInfo,
                FirstImageUrl = dto.ImageUrl,
                Homepage = dto.Homepage,
                AnimalCmgCl = dto.PetAllowed ? "가능" : "불가능",

                // 🌟 Camp.cs 엔티티의 나머지 필수 필드들도 에러 안 나게 채워주기
                LineIntro = "",
                Intro = "",
                DoNm = dto.Address?.Split(' ').FirstOrDefault() ?? "기타",
                SigunguNm = "",
                Addr2 = "",
                ResveUrl = "",
                FeatureNm = "",
                Induty = "일반야영장",
                LctCl = "",
                CreatedTime = System.DateTime.Now,
                ModifiedTime = System.DateTime.Now
            }).ToList(); // 🌟 Select 결과를 리스트로 명확히 변환

            await _repository.SaveCampsAsync(camps);
        }

        public async Task IncreaseViewCountAsync(int id)
        {
            await _repository.IncreaseViewCountAsync(id);
        }
    }
}
