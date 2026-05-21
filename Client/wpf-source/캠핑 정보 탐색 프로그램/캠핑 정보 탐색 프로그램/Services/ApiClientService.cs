using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace 캠핑_정보_탐색_프로그램
{
    public class ApiClientService
    {
        public ObservableCollection<CampDto> GetCamps()
        {
            // DB 연동 후 SELECT 결과 넣는 부분
            return new ObservableCollection<CampDto>();
        }

        public ObservableCollection<CampDto> SearchCamps(string keyword)
        {
            // DB 연동 후 검색 결과 넣는 부분
            return new ObservableCollection<CampDto>();
        }
    }
}
