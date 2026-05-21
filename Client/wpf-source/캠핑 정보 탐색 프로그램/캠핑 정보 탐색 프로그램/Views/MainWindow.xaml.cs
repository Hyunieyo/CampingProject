using System.Windows;

namespace 캠핑_정보_탐색_프로그램
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}