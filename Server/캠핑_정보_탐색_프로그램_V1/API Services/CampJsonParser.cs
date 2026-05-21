using System.Globalization;
using 캠핑_정보_탐색_프로그램_V1.Models.DTO;
using Newtonsoft.Json.Linq;

namespace 캠핑_정보_탐색_프로그램_V1.API_Services
{
    public class CampJsonParser
    {
        public List<CampDto> Parse(string json)
        {
            List<CampDto> campList =
                new List<CampDto>();

            try
            {
                // JSON 문자열 → JObject 변환
                JObject root =
                    JObject.Parse(json);

                // 고캠핑 API item 배열 접근
                var items =
                    root["response"]?["body"]?["items"]?["item"];

                // 데이터 없으면 빈 리스트 반환
                if (items == null)
                    return campList;

                foreach (var item in items)
                {
                    // 위도 변환
                    double.TryParse(
                        item["mapY"]?.ToString(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double lat);

                    // 경도 변환
                    double.TryParse(
                        item["mapX"]?.ToString(),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        out double lng);

                    // 반려동물 허용 여부
                    string petText =
                        item["animalCmgCl"]?.ToString() ?? "";

                    bool isPetAllowed =
                        petText.Contains("가능") &&
                        !petText.Contains("불가능");

                    // DTO 변환
                    CampDto dto = new CampDto
                    {
                        Name =
                            item["facltNm"]?.ToString() ?? "",

                        Address =
                            item["addr1"]?.ToString() ?? "",

                        Tel =
                            item["tel"]?.ToString() ?? "",

                        Latitude = lat,

                        Longitude = lng,

                        FacilityInfo =
                            item["sbrsCl"]?.ToString() ?? "",

                        ImageUrl =
                            item["firstImageUrl"]?.ToString() ?? "",

                        Homepage =
                            item["homepage"]?.ToString() ?? "",

                        PetAllowed =
                            isPetAllowed
                    };

                    campList.Add(dto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[JSON 파싱 오류] {ex.Message}");
            }

            return campList;
        }
    }
}
