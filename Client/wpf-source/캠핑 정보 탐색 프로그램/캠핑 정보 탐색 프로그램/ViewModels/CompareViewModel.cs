using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 캠핑_정보_탐색_프로그램
{
    public class CompareViewModel : BaseViewModel
    {
        public ObservableCollection<CampDto> CompareList { get; set; }

        public ICommand RemoveCompareCommand { get; set; }

        public CompareViewModel()
        {
            CompareList = new ObservableCollection<CampDto>();

            RemoveCompareCommand = new RelayCommand(o =>
            {
                CampDto camp = o as CampDto;

                if (camp != null)
                {
                    CompareList.Remove(camp);
                }
            });
        }
    }
}