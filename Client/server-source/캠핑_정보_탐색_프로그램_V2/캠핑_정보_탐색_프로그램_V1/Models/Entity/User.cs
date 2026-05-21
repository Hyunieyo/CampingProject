using System.ComponentModel.DataAnnotations;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Entity
{
    public class User
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty; // 로그인 아이디 (기본키)

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty; // 비밀번호

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty; // 사용자 이름/닉네임
    }
}
