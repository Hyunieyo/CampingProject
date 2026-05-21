using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using 캠핑_정보_탐색_프로그램.Views;

namespace 캠핑_정보_탐색_프로그램
{
    public class MainViewModel : BaseViewModel
    {
        private ApiClientService api = new ApiClientService();

        private UserControl currentView;

        public UserControl CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        private bool isLogin = false;

        public string LoginButtonText
        {
            get
            {
                if (isLogin == true)
                    return "🚪 로그아웃";
                else
                    return "👤 로그인";
            }
        }

        public ObservableCollection<CampDto> Camps { get; set; }
        public ObservableCollection<CampDto> FavoriteCamps { get; set; }
        public ObservableCollection<CampDto> CompareCamps { get; set; }

        public ICommand MoveSearchCommand { get; set; }
        public ICommand MoveFavoriteCommand { get; set; }
        public ICommand MoveCompareCommand { get; set; }
        public ICommand LoginMenuCommand { get; set; }

        public ICommand LoginSuccessCommand { get; set; }
        public ICommand RemoveCompareCommand { get; set; }

        public MainViewModel()
        {
            Camps = api.GetCamps();

            FavoriteCamps = new ObservableCollection<CampDto>();
            CompareCamps = new ObservableCollection<CampDto>();

            MoveSearchCommand = new RelayCommand(o =>
            {
                CurrentView = new SearchView();
            });

            MoveFavoriteCommand = new RelayCommand(o =>
            {
                CurrentView = new FavoriteView();
            });

            MoveCompareCommand = new RelayCommand(o =>
            {
                CurrentView = new CompareView();
            });

            LoginMenuCommand = new RelayCommand(o =>
            {
                if (isLogin == false)
                {
                    CurrentView = new LoginView();
                }
                else
                {
                    isLogin = false;
                    OnPropertyChanged(nameof(LoginButtonText));

                    MessageBox.Show("로그아웃 되었습니다.");
                    CurrentView = new SearchView();
                }
            });

            LoginSuccessCommand = new RelayCommand(o =>
            {
                isLogin = true;
                OnPropertyChanged(nameof(LoginButtonText));

                MessageBox.Show("로그인 성공");
                CurrentView = new SearchView();
            });

            RemoveCompareCommand = new RelayCommand(o =>
            {
                CampDto camp = o as CampDto;

                if (camp != null)
                {
                    CompareCamps.Remove(camp);
                }
            });

            CurrentView = new SearchView();
        }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}