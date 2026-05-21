using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using 캠핑_정보_탐색_프로그램_V1.API_Services;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface; // 🌟 추가
using 캠핑_정보_탐색_프로그램_V1.Services.Utility;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class CampDataService : ICampDataService // 🌟 인터페이스 연결
    {
        private readonly CampApiService _apiService;
        private readonly DataCleaner _cleaner;

        // 🌟 외부(Program.cs)에서 부품을 주입받도록 생성자 변경!
        public CampDataService(CampApiService apiService, DataCleaner cleaner)
        {
            _apiService = apiService;
            _cleaner = cleaner;
        }

        public async Task<List<CampDto>> SearchCampAsync(string keyword)
        {
            string json = await _apiService.SearchCampJsonAsync(keyword);

            using JsonDocument doc = JsonDocument.Parse(json);
            var items = doc.RootElement.GetProperty("response").GetProperty("body").GetProperty("items").GetProperty("item");

            List<CampDto> camps = new List<CampDto>();

            foreach (var item in items.EnumerateArray())
            {
                CampDto camp = new CampDto();

                // 승빈님이 만든 DataCleaner 알차게 써먹기!
                camp.Name = _cleaner.CleanString(item.GetProperty("facltNm").GetString());
                camp.Address = _cleaner.CleanString(item.GetProperty("addr1").GetString());
                camp.Tel = _cleaner.CleanString(item.GetProperty("tel").GetString());
                camp.FacilityInfo = _cleaner.CleanString(item.GetProperty("sbrsCl").GetString());
                camp.ImageUrl = _cleaner.CleanString(item.GetProperty("firstImageUrl").GetString());
                camp.Homepage = _cleaner.CleanString(item.GetProperty("homepage").GetString());
                camp.PetAllowed = _cleaner.ConvertPet(item.GetProperty("animalCmgCl").GetString());

                // 위도 경도 파싱 에러 방지를 위해 안전하게 처리
                double.TryParse(item.GetProperty("mapY").GetString(), out double lat);
                double.TryParse(item.GetProperty("mapX").GetString(), out double lng);
                camp.Latitude = lat;
                camp.Longitude = lng;

                camps.Add(camp);
            }

            return camps;
        }
    }
}