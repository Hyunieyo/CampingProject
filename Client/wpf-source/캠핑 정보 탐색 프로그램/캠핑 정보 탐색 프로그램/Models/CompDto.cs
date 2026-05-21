using System;
using System.Collections.Generic;

namespace 캠핑_정보_탐색_프로그램
{
    public class CampDto
    {
        public string Name { get; set; }           //캠핑장 이름
        public string Address { get; set; }        //캠핑장 주소
        public string Tel { get; set; }            //캠핑장 전화번호
        public double Latitude { get; set; }       //위도
        public double Longitude { get; set; }      //경도
        public string FacilityInfo { get; set; }   //시설 정보
        public string ImageUrl { get; set; }       //캠핑장 사진 주소
        public string Homepage { get; set; }       //캠핑장 홈페이지 주소
        public bool PetAllowed { get; set; }       //반려동물 가능 여부

        public int TotalSiteCount { get; set; }
        public string DistanceText { get; set; }
    }
}
