using System.Linq;
using System.Windows;
using System.Windows.Input;
using ProductsClient.Classes;

namespace ProductsClient.ViewModel
{
    public class NavigationVM : BaseVM
    {
        private object _currentView;

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            ProductsCommand = new RelayCommand(Products);
            CloseAppCommand = new RelayCommand(Close);
            BindingTelegramCommand = new RelayCommand(BindingTelegram);
            SellProductsCommand = new RelayCommand(SellProduct);

            //Sturtup Page
            HomeCommand.Execute(null);
            //CurrentView = new HomeVM();
        }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand SellProductsCommand { get; set; }
        public ICommand CloseAppCommand { get; set; }

        public ICommand BindingTelegramCommand { get; set; }

        public void Home(object obj) => CurrentView = new HomeVM();
        public void Products(object obj) => CurrentView = new ProductsVM();
        public void SellProduct(object obj) => CurrentView = new SellProductsVM();
        public void Close(object obj)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void BindingTelegram(object obj)
        {
            MessageBox.Show("Пока не готово :(");
        }
    }
}
