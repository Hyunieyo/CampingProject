using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Entity
{
    public class SearchHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SearchHistoryId { get; set; }

        [Required]
        [StringLength(30)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Keyword { get; set; } = string.Empty;

        public DateTime SearchedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

    }
}
