using ProductsClient.Classes;
using ProductsClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ProductsClient.ViewModel
{
    public class SellProductsVM : BaseVM
    {
        private static ICollectionView _currentSaleProductsView;
        private static BindingList<SaleProduct> _currentSaleProducts;

        private SaleProduct _saleProductSelected;

        private static float _totalSum;

        public SellProductsVM()
        {
            CurrentSaleProducts = CurrentSaleProducts ?? new BindingList<SaleProduct>();
            CurrentSaleProductsView = CollectionViewSource.GetDefaultView(CurrentSaleProducts);
            CurrentSaleProducts.ListChanged += (s, a) => { TotalSum = CurrentSaleProducts.Select(x => x.Count * x.Product.SellCost).Sum(); };

            AddProductCommand = new RelayCommand(AddProduct);
            RemoveProductCommand = new RelayCommand(RemoveProduct);
            SellProductCommand = new RelayCommand(SellProduct);
            LoadDataAsyncCommand = new RelayCommand(LoadDataAsync,
                (obj) => Remains == null ||
                         SaleProducts == null ||
                         Receipts == null);
        }

        public ICollectionView CurrentSaleProductsView
        {
            get => _currentSaleProductsView;
            set { _currentSaleProductsView = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ArrivalProduct> Remains
        {
            get => DataInfo.Remains;
            set { DataInfo.Remains = value; OnPropertyChanged(); }
        }
        public BindingList<SaleProduct> CurrentSaleProducts
        {
            get => _currentSaleProducts;
            set { _currentSaleProducts = value; OnPropertyChanged(); }
        }
        public ObservableCollection<SaleProduct> SaleProducts
        {
            get => DataInfo.SaleProducts;
            set { DataInfo.SaleProducts = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Receipt> Receipts
        {
            get => DataInfo.Receipts;
            set { DataInfo.Receipts = value; OnPropertyChanged(); }
        }

        public SaleProduct SaleProductSelected
        {
            get => _saleProductSelected;
            set { _saleProductSelected = value; OnPropertyChanged(); }
        }

        public float TotalSum
        {
            get => _totalSum;
            set { _totalSum = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataAsyncCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand SellProductCommand { get; set; }

        private async void LoadDataAsync(object obj)
        {
            if (DataInfo.ArrivalProducts == null)
            {
                DataInfo.ArrivalProducts = await DataInfo.Instance.GetQueryAsync<ObservableCollection<ArrivalProduct>>(DataInfo.ArrivalProductsLink);
            }

            Remains = await DataInfo.Instance.GetQueryAsync<ObservableCollection<ArrivalProduct>>(DataInfo.RemainsLink);
            Receipts = await DataInfo.Instance.GetQueryAsync<ObservableCollection<Receipt>>(DataInfo.ReceiptsLink);
        }
        private void AddProduct(object obj)
        {
            if (!CurrentSaleProducts.Any(x => x.Product.Id.Equals((uint)obj)))
            {
                SaleProduct sale = new SaleProduct()
                {
                    Product = Remains.First(x => x.Id.Equals((uint)obj)),
                    Count = 1
                };

                CurrentSaleProducts.Add(sale);
            }
            else
            {
                CurrentSaleProducts.First(x => x.Product.Id.Equals((uint)obj)).Count++;
            }
        }
        private void RemoveProduct(object obj)
        {
            CurrentSaleProducts.Remove(obj as SaleProduct);
        }
        private async void SellProduct(object obj)
        {
            TimeSpan test = DateTime.Now.TimeOfDay;
            Receipt receipt = new Receipt()
            {
                Id = Receipts.Count == 0 ? 1 : Receipts.Max(x => x.Id) + 1,
                Employee = DataInfo.User.Employee,
                Date = new DateTime(DateTime.Now.Ticks)
            };

            Receipts.Add(receipt);
            uint lastId = SaleProducts.Count == 0 ? 1 : SaleProducts.Max(y => y.Id) + 1;
            CurrentSaleProducts.ToList().ForEach(x =>
            {
                x.Id = lastId++;
                x.Receipt = receipt;
                SaleProducts.Add(x);
            });

            List<SaleProduct> temp = CurrentSaleProducts.ToList();
            CurrentSaleProducts.Clear();
            temp.AsParallel().ForAll(x => Remains.First(y => y.Id.Equals(x.Product.Id)).Count -= x.Count);

            await DataInfo.Instance.SendPostQueryAsync(temp, DataInfo.SaleProductsLink);
        }
    }
}