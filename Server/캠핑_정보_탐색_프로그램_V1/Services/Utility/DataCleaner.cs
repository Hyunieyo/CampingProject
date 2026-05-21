namespace 캠핑_정보_탐색_프로그램_V1.Services.Utility
{
    public class DataCleaner
    {
        public string CleanString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "정보없음";
            }
            return value.Trim();
        }

        public bool ConvertPet(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();

            return value.StartsWith("가능"); //"가능으로 시작하는가?"
        }

        public string CleanRegion(string doNm)
        {
            if (string.IsNullOrWhiteSpace(doNm))
                return "기타";

            string region = doNm.Trim();

            // 뒤죽박죽인 지역명을 매핑해서 하나로 통일합니다.
            return region switch
            {
                "경상남도" or "경남" => "경남",
                "경상북도" or "경북" => "경북",
                "전라남도" or "전남" => "전남",
                "전라북도" or "전북" => "전북",
                "충청남도" or "충남" => "충남",
                "충청북도" or "충북" => "충북",
                "강원도" or "강원" => "강원",
                "경기도" or "경기" => "경기",
                "제주특별자치도" or "제주도" or "제주" => "제주",
                "서울특별시" or "서울시" => "서울",
                "부산광역시" or "부산시" => "부산",
                "대구광역시" or "대구시" => "대구",
                "인천광역시" or "인천시" => "인천",
                "광주광역시" or "광주시" => "광주",
                "대전광역시" or "대전시" => "대전",
                "울산광역시" or "울산시" => "울산",
                "세종특별자치시" or "세종시" or "세종" => "세종",
                _ => region // 그 외의 경우는 원래 문자열 유지
            };
        }
    }
}
