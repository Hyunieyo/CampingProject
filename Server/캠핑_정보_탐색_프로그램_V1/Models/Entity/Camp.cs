using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace 캠핑_정보_탐색_프로그램_V1.Models.Entity
{
    public class Camp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContentId { get; set; } // 고캠핑 contentId

        [StringLength(100)]
        public string FacltNm { get; set; } // 캠핑장 이름

        [StringLength(300)]
        public string LineIntro { get; set; } // 한줄소개
        public string Intro { get; set; } // 상세소개

        [StringLength(50)]
        public string DoNm { get; set; } // 도 (필터링 기준!)

        [StringLength(50)]
        public string SigunguNm { get; set; } // 시군구

        [StringLength(200)]
        public string Addr1 { get; set; } // 주소

        [StringLength(200)]
        public string Addr2 { get; set; } // 상세주소
        public double MapX { get; set; } // 경도
        public double MapY { get; set; } // 위도

        [StringLength(50)]
        public string Tel { get; set; } // 전화번호

        [StringLength(300)]
        public string Homepage { get; set; } // 홈페이지

        [StringLength(300)]
        public string ResveUrl { get; set; } // 예약 링크
        public string FeatureNm { get; set; } // 특징

        [StringLength(100)]
        public string Induty { get; set; } // 업종

        [StringLength(50)]
        public string LctCl { get; set; } // 입지구분

        [StringLength(50)]
        public string AnimalCmgCl { get; set; } // 반려동물 가능 여부
        public string SbrsCl { get; set; } // 부대시설

        public int GlampSiteCo { get; set; } // 글램핑 수
        public int CaravSiteCo { get; set; } // 카라반 수
        public int AutoSiteCo { get; set; } // 오토캠핑 수
        public int GnrlSiteCo { get; set; } // 일반야영장 수

        [StringLength(500)]
        public string FirstImageUrl { get; set; } // 대표 이미지

        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public int ViewCount {  get; set; }
    }
}