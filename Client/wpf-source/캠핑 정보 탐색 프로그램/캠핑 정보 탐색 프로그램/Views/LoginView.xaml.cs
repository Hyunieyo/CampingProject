using System.Windows;
using System.Windows.Controls;

namespace 캠핑_정보_탐색_프로그램.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void txtPW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPW.Password.Length > 0)
            {
                txtPWPlaceholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPWPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}