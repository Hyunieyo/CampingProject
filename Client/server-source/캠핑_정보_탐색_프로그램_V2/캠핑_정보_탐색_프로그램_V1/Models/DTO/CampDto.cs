namespace 캠핑_정보_탐색_프로그램_V1.Models.DTO
{
    public class CampDto
    {
        public int Id { get; set; }

        public string Name { get; set; } //facltNm

        public string Address { get; set; } //addr1

        public string Tel { get; set; } //tel

        public double Latitude { get; set; } //mapY

        public double Longitude { get; set; } //mapX

        public string FacilityInfo { get; set; } //sbrsCl

        public string ImageUrl { get; set; } //firstImageUrl

        public string Homepage { get; set; } //homepage

        public bool PetAllowed { get; set; } //animalCmgCl
    }
}
