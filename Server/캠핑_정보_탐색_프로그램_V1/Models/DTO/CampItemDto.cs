namespace 캠핑_정보_탐색_프로그램_V1.Models.DTO
{
    public class CampItemDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string ItemName { get; set; }
        public double RecommendedQuantity { get; set; }
        public string Unit { get; set; }
        public bool IsChecked { get; set; }
    }
}
