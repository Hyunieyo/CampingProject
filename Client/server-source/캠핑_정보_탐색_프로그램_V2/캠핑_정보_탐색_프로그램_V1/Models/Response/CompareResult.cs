using 캠핑_정보_탐색_프로그램_V1.Models.Entity;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Response
{
    public class CompareResult
    {
        // 1. 선택된 캠핑장들의 상세 정보 리스트를 담아 보낼 바구니
        public List<Camp> CompareList { get; set; } = new List<Camp>();

        // 2. 화면에 특별한 안내 메시지를 띄워주거나 오류를 알릴 때 쓸 필드
        public string CompareMessage { get; set; } = string.Empty;
    }
}
