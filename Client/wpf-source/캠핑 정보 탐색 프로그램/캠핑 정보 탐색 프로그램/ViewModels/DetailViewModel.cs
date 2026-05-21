using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 캠핑_정보_탐색_프로그램
{
    public class DetailViewModel : MainViewModel
    {
        private CampDto selectedCamp;

        public CampDto SelectedCamp
        {
            get { return selectedCamp; }
            set
            {
                selectedCamp = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReserveCommand { get; set; }

        public DetailViewModel(CampDto camp)
        {
            SelectedCamp = camp;

            ReserveCommand = new RelayCommand(o =>
            {
                // 예약 시뮬레이션 처리 부분
            });
        }
    }
}
