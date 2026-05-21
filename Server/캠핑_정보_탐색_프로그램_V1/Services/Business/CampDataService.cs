using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using 캠핑_정보_탐색_프로그램_V1.API_Services;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;
using 캠핑_정보_탐색_프로그램_V1.Services.Utility;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class CampDataService : ICampDataService
    {
        private const int PageSize = 100;

        private readonly CampApiService _apiService;
        private readonly DataCleaner _cleaner;

        public CampDataService(CampApiService apiService, DataCleaner cleaner)
        {
            _apiService = apiService;
            _cleaner = cleaner;
        }

        public async Task<List<CampDto>> SearchCampAsync(string keyword)
        {
            string json = await _apiService.SearchCampJsonAsync(keyword);
            return ParseCampJson(json);
        }

        public async Task<List<CampDto>> GetAllCampAsync()
        {
            List<CampDto> allCamps = new List<CampDto>();
            int pageNo = 1;
            int totalCount = 0;

            while (true)
            {
                string json = await _apiService.GetCampListJsonAsync(pageNo, PageSize);

                using JsonDocument doc = JsonDocument.Parse(json);
                var body = doc.RootElement.GetProperty("response").GetProperty("body");

                if (pageNo == 1)
                    int.TryParse(body.GetProperty("totalCount").ToString(), out totalCount);

                allCamps.AddRange(ParseCampJson(json));

                if (totalCount == 0 || allCamps.Count >= totalCount)
                    break;

                pageNo++;
                await Task.Delay(150);
            }

            return allCamps;
        }

        private List<CampDto> ParseCampJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            var body = doc.RootElement.GetProperty("response").GetProperty("body");

            List<CampDto> camps = new List<CampDto>();

            if (!body.TryGetProperty("items", out JsonElement items) ||
                !items.TryGetProperty("item", out JsonElement itemElement))
            {
                return camps;
            }

            if (itemElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in itemElement.EnumerateArray())
                    camps.Add(CreateCampDto(item));
            }
            else if (itemElement.ValueKind == JsonValueKind.Object)
            {
                camps.Add(CreateCampDto(itemElement));
            }

            return camps;
        }

        private CampDto CreateCampDto(JsonElement item)
        {
            CampDto camp = new CampDto();

            camp.Id = GetInt(item, "contentId");
            camp.Name = _cleaner.CleanString(GetString(item, "facltNm"));
            camp.DoNm = _cleaner.CleanRegion(GetString(item, "doNm"));
            camp.Address = _cleaner.CleanString(GetString(item, "addr1"));
            camp.Tel = _cleaner.CleanString(GetString(item, "tel"));
            camp.FacilityInfo = _cleaner.CleanString(GetString(item, "sbrsCl"));
            camp.ImageUrl = _cleaner.CleanString(GetString(item, "firstImageUrl"));
            camp.Homepage = _cleaner.CleanString(GetString(item, "homepage"));
            camp.PetAllowed = _cleaner.ConvertPet(GetString(item, "animalCmgCl"));

            double.TryParse(GetString(item, "mapY"), out double lat);
            double.TryParse(GetString(item, "mapX"), out double lng);
            camp.Latitude = lat;
            camp.Longitude = lng;

            return camp;
        }

        private static string GetString(JsonElement item, string propertyName)
        {
            if (!item.TryGetProperty(propertyName, out JsonElement value))
                return string.Empty;

            return value.ValueKind == JsonValueKind.String
                ? value.GetString() ?? string.Empty
                : value.ToString();
        }

        private static int GetInt(JsonElement item, string propertyName)
        {
            if (!item.TryGetProperty(propertyName, out JsonElement value))
                return 0;

            int.TryParse(value.ToString(), out int number);
            return number;
        }
    }
}