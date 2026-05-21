namespace 캠핑_정보_탐색_프로그램_V1.API_Services
{
    public class CampApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // 1. 클래스 상단 필드 선언부에서 초기값 지정을 해줍니다. (또는 하단 생성자 수정)
        private readonly string apiUrl = string.Empty;
        private readonly string apiKey = string.Empty;

        public CampApiService()
        {
            // appsettings.json 파일을 빌드하고 읽어오는 설정단
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // optional: true로 수정 확인!
                .Build();

            // 뒤에 ?? "" 를 붙여서 혹시라도 null이 오면 빈 문자열("")로 대체되게 만듭니다.
            apiUrl = config["CampApiSettings:Url"] ?? string.Empty;
            apiKey = config["CampApiSettings:Key"] ?? string.Empty;
        }

        private string CreateUrl(string operation, string additionalParams, int pageNo = 1, int numOfRows = 100)
        {
            string defaultParams =
                $"?serviceKey={apiKey}" +
                $"&numOfRows={numOfRows}" +
                $"&pageNo={pageNo}" +
                $"&MobileOS=ETC" +
                $"&MobileApp=CampingApp";

            return $"{apiUrl}/{operation}{defaultParams}{additionalParams}";
        }

        /// <summary>
        /// 키워드 검색 XML 반환
        /// </summary>
        public async Task<string> SearchCampJsonAsync(string keyword, int pageNo = 1, int numOfRows = 100)
        {
            string encodedKeyword =
                Uri.EscapeDataString(keyword);

            string fullUrl =
                 CreateUrl(
                     "searchList",
                      $"&keyword={encodedKeyword}&_type=json",
                      pageNo,
                      numOfRows);

            return await _httpClient.GetStringAsync(fullUrl);
        }

        /// <summary>
        /// 전체 캠핑장 목록 JSON 반환
        /// </summary>
        public async Task<string> GetCampListJsonAsync(int pageNo = 1, int numOfRows = 100)
        {
            string fullUrl =
                CreateUrl(
                    "basedList",
                    "&_type=json",
                    pageNo,
                    numOfRows);

            return await _httpClient.GetStringAsync(fullUrl);
        }

        /// <summary>
        /// 위치 기반 XML 반환
        /// </summary>
        public async Task<string> SearchLocationJsonAsync(
            double mapX,
            double mapY,
            int radius = 2000)
        {
            string locationParams =
                 $"&mapX={mapX}&mapY={mapY}&radius={radius}&_type=json";

            string fullUrl =
                CreateUrl(
                    "locationBasedList",
                    locationParams);

            return await _httpClient.GetStringAsync(fullUrl);
        }
    }
}