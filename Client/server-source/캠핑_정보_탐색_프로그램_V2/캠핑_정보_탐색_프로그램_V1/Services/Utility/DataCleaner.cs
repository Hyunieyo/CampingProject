namespace 캠핑_정보_탐색_프로그램_V1.Services.Utility
{
    public class DataCleaner
    {
        public string CleanString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "정보없음";
            }
            return value.Trim();
        }

        public bool ConvertPet(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim();

            return value.StartsWith("가능"); //"가능으로 시작하는가?"
        }
    }
}
