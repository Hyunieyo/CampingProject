using 캠핑_정보_탐색_프로그램_V1.Models.Entity;
using 캠핑_정보_탐색_프로그램_V1.Models.Response;

namespace 캠핑_정보_탐색_프로그램_V1.Services.Interface
{
    public interface ICompareService
    {
        CompareResult CompareCamps(List<Camp> camps);
    }
}
