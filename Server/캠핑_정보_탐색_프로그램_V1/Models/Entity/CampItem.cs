using System.ComponentModel.DataAnnotations;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Entity
{
    public class CampItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } // 식재료, 장비, 위생 등

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }     // 품목명 (삼겹살)

        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }     // 단위 (kg, 개)

        [Required]
        public double PerAdultAmount { get; set; } // 1인당 기준량
    }
}