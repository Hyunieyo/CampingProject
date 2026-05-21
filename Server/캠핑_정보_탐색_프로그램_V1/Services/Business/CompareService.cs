using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Models.Response;
using 캠핑_정보_탐색_프로그램_V1.Services.Interface;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Business
{
    public class CompareService : ICompareService
    {
        public CompareResult CompareCamps(List<Camp> camps)
        {
            var result = new CompareResult();

            if (camps == null || camps.Count == 0)
            {
                result.CompareMessage = "비교할 캠핑장이 선택되지 않았습니다.";
                return result;
            }

            // DTO 상자에 비교할 캠핑장 리스트를 그대로 담아줍니다.
            result.CompareList = camps;
            result.CompareMessage = $"{camps.Count}개의 캠핑장 비교 데이터가 준비되었습니다.";

            // 💡 (선택) 나중에 여기에 "반려동물 동반 가능한 곳은 X개입니다" 같은 
            // 백엔드만의 특수 가공 로직을 추가할 수도 있습니다!

            return result;
        }
    }
}
