using ProductsClient.Classes;
using System.Data;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using ProductsClient.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;
using System.Threading;

namespace ProductsClient.ViewModel
{
    public class ProductsVM : BaseVM
    {
        private static ICollectionView _arrivalProductsView;

        private Product _productSelected;
        private NoteProduct _noteProductSelected;
        private ArrivalProduct _arrivalProductSelected;
        private ProductGroup _productGroupSelected;
        private ProductUnit _productUnitSelected;
        private Manufacture _manufactureSelected;
        private Country _countrySelected;
        private Supplier _supplierSelected;

        private ArrivalProduct _currentArrivalProduct;
        private NoteProduct _currentNoteProduct;
        private Product _currentProduct;

        private Visibility _menuItemAddVisible;
        private string _filterDataText;
        private uint _overPrice;

        public ProductsVM()
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy HH:mm";
            Thread.CurrentThread.CurrentCulture = ci;
            MenuItemAddVisible = Visibility.Collapsed;

            MenuAddOpenCommand = new RelayCommand(MenuAddOpen, (obj) => ArrivalProducts != null);
            MenuAddCloseCommand = new RelayCommand(MenuAddClose);
            CountryAddCommand = new RelayCommand(CountryAddAsync);
            ManufactureAddCommand = new RelayCommand(ManufactureAddAsync);
            ProductGroupAddCommand = new RelayCommand(ProductGroupAddAsync);
            ProductUnitAddCommand = new RelayCommand(ProductUnitAddAsync);
            SupplierAddCommand = new RelayCommand(SupplierAddAsync);
            ProductAddCommand = new RelayCommand(ProductAddAsync);
            NoteProductAddCommand = new RelayCommand(NoteProductAddAsync);
            ArrivalProductAddCommand = new RelayCommand(ArrivalProductAddAsync);
            LoadDataAsyncCommand = new RelayCommand(LoadDataAsync,
                (obj) => ArrivalProducts == null ||
                        NoteProducts == null ||
                        Products == null ||
                        ProductGroups == null ||
                        ProductUnits == null ||
                        Manufactures == null ||
                        Countries == null ||
                        Suppliers == null);

            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += (s, a) => { LoadDataAsyncCommand.Execute(null); };
            //worker.RunWorkerAsync();
        }

        public ICollectionView ArrivalProductsView
        {
            get => _arrivalProductsView;
            set { _arrivalProductsView = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ArrivalProduct> ArrivalProducts
        {
            get => DataInfo.ArrivalProducts;
            set { DataInfo.ArrivalProducts = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Product> Products
        {
            get => DataInfo.Products;
            set { DataInfo.Products = value; OnPropertyChanged(); }
        }
        public ObservableCollection<NoteProduct> NoteProducts
        {
            get => DataInfo.NoteProducts;
            set { DataInfo.NoteProducts = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ProductGroup> ProductGroups
        {
            get => DataInfo.ProductGroups;
            set { DataInfo.ProductGroups = value; OnPropertyChanged(); }
        }
        public ObservableCollection<ProductUnit> ProductUnits
        {
            get => DataInfo.ProductUnits;
            set { DataInfo.ProductUnits = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Manufacture> Manufactures
        {
            get => DataInfo.Manufactures;
            set { DataInfo.Manufactures = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Country> Countries
        {
            get => DataInfo.Countries;
            set { DataInfo.Countries = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Supplier> Suppliers
        {
            get => DataInfo.Suppliers;
            set { DataInfo.Suppliers = value; OnPropertyChanged(); }
        }

        public Product ProductSelected
        {
            get => _productSelected;
            set { _productSelected = value; OnPropertyChanged(); }
        }
        public NoteProduct NoteProductSelected
        {
            get => _noteProductSelected;
            set { _noteProductSelected = value; OnPropertyChanged(); }
        }
        public ArrivalProduct ArrivalProductSelected
        {
            get => _arrivalProductSelected;
            set { _arrivalProductSelected = value; OnPropertyChanged(); }
        }
        public ProductGroup ProductGroupSelected
        {
            get => _productGroupSelected;
            set { _productGroupSelected = value; OnPropertyChanged(); }
        }
        public ProductUnit ProductUnitSelected
        {
            get => _productUnitSelected;
            set { _productUnitSelected = value; OnPropertyChanged(); }
        }
        public Manufacture ManufactureSelected
        {
            get => _manufactureSelected;
            set { _manufactureSelected = value; OnPropertyChanged(); }
        }
        public Country CountrySelected
        {
            get => _countrySelected;
            set { _countrySelected = value; OnPropertyChanged(); }
        }
        public Supplier SupplierSelected
        {
            get => _supplierSelected;
            set { _supplierSelected = value; OnPropertyChanged(); }
        }

        public ArrivalProduct CurrentArrivalProduct
        {
            get => _currentArrivalProduct ?? (_currentArrivalProduct = new ArrivalProduct());
            set { _currentArrivalProduct = value; OnPropertyChanged(); }
        }
        public NoteProduct CurrentNoteProduct
        {
            get => _currentNoteProduct ?? (_currentNoteProduct = new NoteProduct());
            set { _currentNoteProduct = value; OnPropertyChanged(); }
        }
        public Product CurrentProduct
        {
            get => _currentProduct ?? (_currentProduct = new Product());
            set { _currentProduct = value; OnPropertyChanged(); }
        }

        public Visibility MenuItemAddVisible
        {
            get => _menuItemAddVisible;
            set { _menuItemAddVisible = value; OnPropertyChanged(); }
        }
        public string FilterDataText
        {
            get => _filterDataText;
            set { _filterDataText = value; OnPropertyChanged(); ArrivalProductsView.Filter = (obj) => { return obj is ArrivalProduct arrival && arrival.ToString().Contains(FilterDataText); }; }
        }
        public uint OverPrice
        {
            get => _overPrice;
            set { _overPrice = value; OnPropertyChanged(); CurrentArrivalProduct.SellCost = CurrentArrivalProduct.PurchaseCost + (CurrentArrivalProduct.PurchaseCost * OverPrice / 100); OnPropertyChanged(nameof(CurrentArrivalProduct)); }
        }

        public ICommand MenuAddOpenCommand { get; set; }
        public ICommand MenuAddCloseCommand { get; set; }
        public ICommand LoadDataAsyncCommand { get; set; }
        public ICommand CountryAddCommand { get; set; }
        public ICommand ManufactureAddCommand { get; set; }
        public ICommand ProductGroupAddCommand { get; set; }
        public ICommand ProductUnitAddCommand { get; set; }
        public ICommand SupplierAddCommand { get; set; }
        public ICommand ProductAddCommand { get; set; }
        public ICommand NoteProductAddCommand { get; set; }
        public ICommand ArrivalProductAddCommand { get; set; }

        private void MenuAddOpen(object obj)
        {
            if (obj is ArrivalProduct arrivalProduct)
            {
                CurrentArrivalProduct = new ArrivalProduct(arrivalProduct);
                CurrentNoteProduct = NoteProducts.First(note => note.Id == arrivalProduct.Note.Id);
                CurrentProduct = Products.First(product => product.IdName == arrivalProduct.Product.IdName);

                NoteProductSelected = CurrentNoteProduct;
                ProductSelected = CurrentProduct;
                CountrySelected = Countries.First(country => country.Id == CurrentProduct.Manufacture.Country.Id);
                ManufactureSelected = Manufactures.First(manufacture => manufacture.Id == CurrentProduct.Manufacture.Id);
                SupplierSelected = Suppliers.First(supplier => supplier.Id == CurrentNoteProduct.Supplier.Id);
            }

            MenuItemAddVisible = Visibility.Visible;
        }
        private void MenuAddClose(object obj)
        {
            if (obj is string text && (text == "Save"))
            {
                CurrentArrivalProduct = new ArrivalProduct();
                CurrentNoteProduct = new NoteProduct();
                CurrentProduct = new Product();

                NoteProductSelected = null;
                ProductSelected = null;
                CountrySelected = null;
                ManufactureSelected = null;
                SupplierSelected = null;
            }

            MenuItemAddVisible = Visibility.Collapsed;
        }
        private async void LoadDataAsync(object obj)
        {
            Products = await DataInfo.Instance.GetQueryAsync<ObservableCollection<Product>>(DataInfo.ProductsLink);
            NoteProducts = await DataInfo.Instance.GetQueryAsync<ObservableCollection<NoteProduct>>(DataInfo.NoteProductsLink);
            ProductGroups = await DataInfo.Instance.GetQueryAsync<ObservableCollection<ProductGroup>>(DataInfo.ProductGroupsLink);
            ProductUnits = await DataInfo.Instance.GetQueryAsync<ObservableCollection<ProductUnit>>(DataInfo.ProductUnitsLink);
            Manufactures = await DataInfo.Instance.GetQueryAsync<ObservableCollection<Manufacture>>(DataInfo.ManufacturesLink);
            Countries = await DataInfo.Instance.GetQueryAsync<ObservableCollection<Country>>(DataInfo.CountriesLink);
            Suppliers = await DataInfo.Instance.GetQueryAsync<ObservableCollection<Supplier>>(DataInfo.SuppliersLink);

            if (ArrivalProducts == null)
            {
                await DataInfo.Instance.GetQueryAsync<ObservableCollection<ArrivalProduct>>(DataInfo.ArrivalProductsLink).ContinueWith((x) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ArrivalProducts = x.Result;
                        ArrivalProductsView = CollectionViewSource.GetDefaultView(ArrivalProducts);
                        ArrivalProductsView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
                    });
                });
            }
            else
            {
                ArrivalProductsView = CollectionViewSource.GetDefaultView(ArrivalProducts);
                ArrivalProductsView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }

            if (DataInfo.Remains == null)
            {
                DataInfo.Remains = await DataInfo.Instance.GetQueryAsync<ObservableCollection<ArrivalProduct>>(DataInfo.RemainsLink);
            }
        }

        private async void CountryAddAsync(object obj)
        {
            if (!Countries.Any(country =>
                country.Id.Equals(CurrentProduct.Manufacture.Country.Id) ||
                country.IdName.Equals(CurrentProduct.Manufacture.Country.IdName) ||
                country.ShortName.Equals(CurrentProduct.Manufacture.Country.ShortName) ||
                country.FullName.Equals(CurrentProduct.Manufacture.Country.FullName)))
            {
                Countries.Add(new Country(CurrentProduct.Manufacture.Country));
                CountrySelected = Countries.Last();

                await DataInfo.Instance.SendPostQueryAsync(CountrySelected, DataInfo.CountriesLink);
            }
            else
            {
                CountrySaveAsync(obj);
            }
        }
        private async void CountrySaveAsync(object obj)
        {
            IEnumerable<Country> rows = Countries.Where(country =>
                country.Id.Equals(CurrentProduct.Manufacture.Country.Id) ||
                country.IdName.Equals(CurrentProduct.Manufacture.Country.IdName) ||
                country.ShortName.Equals(CurrentProduct.Manufacture.Country.ShortName) ||
                country.FullName.Equals(CurrentProduct.Manufacture.Country.FullName));

            if (rows.Count() == 1)
            {
                if (MessageBox.Show($"Внести изменения в страну '{rows.First().ShortName} ({rows.First().Id} | {rows.First().IdName})'?", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Countries.Where(x => x.Id.Equals(rows.First().Id)).ToList().ForEach(x => { Countries.Remove(x); Countries.Add(new Country(CurrentProduct.Manufacture.Country)); });
                    CountrySelected = Countries.Last();

                    await DataInfo.Instance.SendPutQueryAsync(CurrentProduct.Manufacture.Country, $"{DataInfo.CountriesLink}/{CurrentProduct.Manufacture.Country.Id}");
                }
            }
            else
            {
                MessageBox.Show("Для редактирования страны не должно быть сходств с несколькими одновременно.", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void ManufactureAddAsync(object obj)
        {
            if (CountrySelected != null && !Manufactures.Any(manufacture => manufacture.Name.Equals(CurrentProduct.Manufacture.Name)))
            {
                CurrentProduct.Manufacture.Id = Manufactures.Count == 0 ? 1 : Manufactures.Max(x => x.Id) + 1;
                CurrentProduct.Manufacture.Country = CountrySelected;

                Manufactures.Add(new Manufacture(CurrentProduct.Manufacture));
                ManufactureSelected = Manufactures.Last();

                await DataInfo.Instance.SendPostQueryAsync(ManufactureSelected, DataInfo.ManufacturesLink);
            }
            else
            {
                ManufactureSaveAsync(obj);
            }
        }
        private async void ManufactureSaveAsync(object obj)
        {
            if (CountrySelected != null)
            {
                if (MessageBox.Show($"Внести изменения в производителе '{CurrentProduct.Manufacture.Name}'", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CurrentProduct.Manufacture.Id = Manufactures.First(x => x.Name.Equals(CurrentProduct.Manufacture.Name)).Id;
                    CurrentProduct.Manufacture.Country = CountrySelected;

                    Manufactures.Where(x => x.Id.Equals(CurrentProduct.Manufacture.Id)).ToList().ForEach(x => { Manufactures.Remove(x); Manufactures.Add(new Manufacture(CurrentProduct.Manufacture)); });
                    ManufactureSelected = Manufactures.Last();

                    await DataInfo.Instance.SendPutQueryAsync(CurrentProduct.Manufacture, $"{DataInfo.ManufacturesLink}/{CurrentProduct.Manufacture.Id}");
                }
            }
        }
        private async void ProductGroupAddAsync(object obj)
        {
            if (!ProductGroups.Any(group => group.Name.Equals(CurrentProduct.Group.Name)))
            {
                CurrentProduct.Group.Id = ProductGroups.Count == 0 ? 1 : ProductGroups.Max(x => x.Id) + 1;

                ProductGroups.Add(new ProductGroup(CurrentProduct.Group));
                ProductGroupSelected = ProductGroups.Last();

                await DataInfo.Instance.SendPostQueryAsync(CurrentProduct.Group, DataInfo.ProductGroupsLink);
            }
        }
        private async void ProductUnitAddAsync(object obj)
        {
            if (!ProductUnits.Any(unit => unit.Name.Equals(CurrentProduct.Unit.Name)))
            {
                CurrentProduct.Unit.Id = ProductUnits.Count == 0 ? 1 : ProductUnits.Max(x => x.Id) + 1;
               
                ProductUnits.Add(new ProductUnit(CurrentProduct.Unit));
                ProductUnitSelected = ProductUnits.Last();

                await DataInfo.Instance.SendPostQueryAsync(CurrentProduct.Unit, DataInfo.ProductUnitsLink);
            }
        }
        private async void SupplierAddAsync(object obj)
        {
            if (!Suppliers.Any(supplier => supplier.Name.Equals(CurrentNoteProduct.Supplier.Name)))
            {
                CurrentNoteProduct.Supplier.Id = Suppliers.Count == 0 ? 1 : Suppliers.Max(x => x.Id) + 1;

                Suppliers.Add(new Supplier(CurrentNoteProduct.Supplier));
                SupplierSelected = Suppliers.Last();

                await DataInfo.Instance.SendPostQueryAsync(CurrentNoteProduct.Supplier, DataInfo.SuppliersLink);
            }
            else
            {
                SupplierSaveAsync(obj);
            }
        }
        private async void SupplierSaveAsync(object obj)
        {
            if (MessageBox.Show($"Внести изменения в поставщике '{CurrentNoteProduct.Supplier.Name}'?", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CurrentNoteProduct.Supplier.Id = Suppliers.First(x => x.Name.Equals(CurrentNoteProduct.Supplier.Name)).Id;

                Suppliers.Where(x => x.Id.Equals(CurrentNoteProduct.Supplier.Id)).ToList().ForEach(x => { Suppliers.Remove(x); Suppliers.Add(new Supplier(CurrentNoteProduct.Supplier)); });
                SupplierSelected = Suppliers.Last();

                await DataInfo.Instance.SendPutQueryAsync(CurrentNoteProduct.Supplier, $"{DataInfo.SuppliersLink}/{CurrentNoteProduct.Supplier.Id}");
            }
        }
        private async void ProductAddAsync(object obj)
        {
            if (ProductGroupSelected != null && ProductUnitSelected != null && ManufactureSelected != null && !Products.Any(product => product.IdName.Equals(CurrentProduct.IdName)))
            {
                CurrentProduct.Group = ProductGroupSelected;
                CurrentProduct.Unit = ProductUnitSelected;
                CurrentProduct.Manufacture = ManufactureSelected;

                Products.Add(new Product(CurrentProduct));
                ProductSelected = Products.Last();

                await DataInfo.Instance.SendPostQueryAsync(CurrentProduct, DataInfo.ProductsLink);
            }
            else
            {
                ProductSaveAsync(obj);
            }
        }
        private async void ProductSaveAsync(object obj)
        {
            if (ProductGroupSelected != null && ProductUnitSelected != null && ManufactureSelected != null)
            {
                if (MessageBox.Show($"Внести изменения в товар '{CurrentProduct.IdName}'?", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CurrentProduct.Group = ProductGroupSelected;
                    CurrentProduct.Unit = ProductUnitSelected;
                    CurrentProduct.Manufacture = ManufactureSelected;

                    Products.Where(x => x.IdName.Equals(CurrentProduct.IdName)).ToList().ForEach(x => { Products.Remove(x); Products.Add(new Product(CurrentProduct)); });
                    ProductSelected = Products.Last();

                    await DataInfo.Instance.SendPutQueryAsync(CurrentProduct, $"{DataInfo.ProductsLink}/{CurrentProduct.IdName}");
                }
            }
        }
        private async void NoteProductAddAsync(object obj)
        {
            if (SupplierSelected != null && !NoteProducts.Any(note => note.Id.Equals(CurrentNoteProduct.Id)))
            {
                CurrentNoteProduct.Employee = DataInfo.User.Employee;
                CurrentNoteProduct.Supplier = SupplierSelected;

                NoteProducts.Add(CurrentNoteProduct);
                NoteProductSelected = NoteProducts.Last();

                await DataInfo.Instance.SendPostQueryAsync(CurrentNoteProduct, DataInfo.NoteProductsLink);
            }
            else
            {
                NoteProductSaveAsync(obj);
            }
        }
        private async void NoteProductSaveAsync(object obj)
        {
            if (SupplierSelected != null)
            {
                if (MessageBox.Show($"Внести изменения в ТТН № '{CurrentNoteProduct.Id}'?", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CurrentNoteProduct.Employee = DataInfo.User.Employee;
                    CurrentNoteProduct.Supplier = SupplierSelected;

                    NoteProducts.Where(x => x.Id.Equals(CurrentNoteProduct.Id)).ToList().ForEach(x => { NoteProducts.Remove(x); NoteProducts.Add(new NoteProduct(CurrentNoteProduct)); });
                    NoteProductSelected = NoteProducts.Last();

                    await DataInfo.Instance.SendPutQueryAsync(CurrentNoteProduct, $"{DataInfo.NoteProductsLink}/{CurrentNoteProduct.Id}");
                }
            }
        }
        private async void ArrivalProductAddAsync(object obj)
        {
            if (CurrentArrivalProduct.Id.Equals(0))
            {
                if (ProductSelected != null && NoteProductSelected != null)
                {
                    CurrentArrivalProduct.Id = ArrivalProducts.Count == 0 ? 1 : ArrivalProducts.Max(arrival => arrival.Id) + 1;
                    CurrentArrivalProduct.Note = NoteProductSelected;
                    CurrentArrivalProduct.Product = ProductSelected;

                    ArrivalProduct current = new ArrivalProduct(CurrentArrivalProduct);
                    ArrivalProducts.Add(CurrentArrivalProduct);
                    DataInfo.Remains.Add(current);
                    MenuAddClose("Save");

                    await DataInfo.Instance.SendPostQueryAsync(current, DataInfo.ArrivalProductsLink);
                }
            }
            else
            {
                ArrivalProductSaveAsync(obj);
            }
        }
        private async void ArrivalProductSaveAsync(object obj)
        {
            if (ProductSelected != null && NoteProductSelected != null)
            {
                if (MessageBox.Show($"Внести изменения в поставку № {CurrentArrivalProduct.Id}", Application.Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CurrentArrivalProduct.Note = NoteProductSelected;
                    CurrentArrivalProduct.Product = ProductSelected;

                    ArrivalProduct current = new ArrivalProduct(CurrentArrivalProduct);
                    DataInfo.Remains.Where(x => x.Id.Equals(CurrentArrivalProduct.Id)).ToList().ForEach(x => { DataInfo.Remains.Remove(x); DataInfo.Remains.Add(current); });
                    ArrivalProducts.Where(x => x.Id.Equals(CurrentArrivalProduct.Id)).ToList().ForEach(x => { ArrivalProducts.Remove(x); ArrivalProducts.Add(CurrentArrivalProduct); });
                    MenuAddClose("Save");

                    await DataInfo.Instance.SendPutQueryAsync(current, $"{DataInfo.ArrivalProductsLink}/{current.Id}");
                }
            }
        }
    }
}