using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Newtonsoft.Json;
using ProductsClient.Classes;
using ProductsClient.Model;

namespace ProductsClient.ViewModel
{
    public class HomeVM : BaseVM
    {
        private DispatcherTimer _timer;
        private string _name;

        public HomeVM()
        {
            //Watching().Start();
            LoadDataAsyncCommand = new RelayCommand(LoadDataAsync, (obj) => SaleProducts == null);
        }

        public ObservableCollection<SaleProduct> SaleProducts
        {
            get => JsonConvert.DeserializeObject<ObservableCollection<SaleProduct>>(JsonConvert.SerializeObject(DataInfo.SaleProducts?.OrderByDescending(x => x.Id)?.Take(5)));
            set { DataInfo.SaleProducts = value; OnPropertyChanged(); }
        }

        public string Greet => GreetText();
        private string EmployeeName => _name ?? (_name = string.Join(" ", DataInfo.User.Employee.Name.Split(' ').Skip(1)));

        public ICommand LoadDataAsyncCommand { get; set; }

        private async void LoadDataAsync(object obj)
        {
            Watching().Start();
            SaleProducts = await DataInfo.Instance.GetQueryAsync<ObservableCollection<SaleProduct>>(DataInfo.SaleProductsLink);
        }

        private DispatcherTimer Watching()
        {
            (_timer ?? (_timer = new DispatcherTimer())).Tick += (s, a) => { OnPropertyChanged(nameof(Greet)); _timer.Interval = TimeSpan.FromSeconds((60 * (60 - DateTime.Now.Minute)) - DateTime.Now.Second); };
            return _timer;
        }
        private string GreetText()
        {
            string text = "";

            switch (DateTime.Now.Hour)
            {
                case int i when i >= 0 && i <= 5: text = $"Доброй ночи, {EmployeeName}!"; break;
                case int i when i >= 6 && i <= 11: text = $"Доброе утро, {EmployeeName}!"; break;
                case int i when i >= 12 && i <= 17: text = $"Добрый день, {EmployeeName}!"; break;
                case int i when i >= 18 && i <= 23: text = $"Добрый вечер, {EmployeeName}!"; break;
            }

            return text;
        }
    }
}
