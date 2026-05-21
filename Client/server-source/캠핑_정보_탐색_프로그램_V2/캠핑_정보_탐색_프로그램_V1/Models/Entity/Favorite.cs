using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Entity
{
    public class Favorite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 1부터 자동으로 늘어나는 고유 번호
        public int FavoriteId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty; // 찜한 유저 ID

        [Required]
        public int CampId { get; set; } // 찜한 캠핑장 ID (Camp 테이블의 기본키와 매칭)

        public DateTime CreatedAt { get; set; } = DateTime.Now; // 찜한 시간

        // 데이터 조회 시 캠핑장 상세 정보까지 한 번에 묶어오기 위한 엔티티 연결 (Navigation Property)
        [ForeignKey("CampId")]
        public virtual Camp Camp { get; set; }
    }
}
