using Newtonsoft.Json;
using ProductsClient.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProductsClient.Classes
{
    public class DataInfo
    {
        private static DataInfo _class;
        private static object _obj = new object();

        public static DataInfo Instance
        {
            get
            {
                if (_class == null)
                {
                    lock (_obj)
                    {
                        if (_class == null)
                            _class = new DataInfo();
                    }
                }

                return _class;
            }
        }

        public static string SaleProductsLink { get; set; } = "https://localhost:44372/api/SaleProducts";
        public static string ReceiptsLink { get; set; } = "https://localhost:44372/api/Receipts";
        public static string ArrivalProductsLink { get; set; } = "https://localhost:44372/api/ArrivalProducts";
        public static string RemainsLink { get; set; } = "https://localhost:44372/api/ArrivalProducts/Remains";
        public static string NoteProductsLink { get; set; } = "https://localhost:44372/api/NoteProducts";
        public static string ProductsLink { get; set; } = "https://localhost:44372/api/Products";
        public static string ProductGroupsLink { get; set; } = "https://localhost:44372/api/ProductGroups";
        public static string ProductUnitsLink { get; set; } = "https://localhost:44372/api/ProductUnits";
        public static string ManufacturesLink { get; set; } = "https://localhost:44372/api/Manufactures";
        public static string CountriesLink { get; set; } = "https://localhost:44372/api/Countries";
        public static string SuppliersLink { get; set; } = "https://localhost:44372/api/Suppliers";
        public static string AuthenticateLink { get; set; } = "https://localhost:44372/api/Authenticate";

        //public static string SaleProductsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/SaleProducts";
        //public static string ReceiptsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Receipts";
        //public static string ArrivalProductsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/ArrivalProducts";
        //public static string RemainsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/ArrivalProducts/Remains";
        //public static string NoteProductsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/NoteProducts";
        //public static string ProductsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Products";
        //public static string ProductGroupsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/ProductGroups";
        //public static string ProductUnitsLink { get; set; } = "https://ychet-tovarov.somee.com/Api/ProductUnits";
        //public static string ManufacturesLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Manufactures";
        //public static string CountriesLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Countries";
        //public static string SuppliersLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Suppliers";
        //public static string AuthenticateLink { get; set; } = "https://ychet-tovarov.somee.com/Api/Authenticate";

        private static string Token { get; set; }
        public static User User { get; private set; }

        public static ObservableCollection<SaleProduct> SaleProducts { get; set; }
        public static ObservableCollection<Receipt> Receipts { get; set; }
        public static ObservableCollection<ArrivalProduct> Remains { get; set; }
        public static ObservableCollection<ArrivalProduct> ArrivalProducts { get; set; }
        public static ObservableCollection<NoteProduct> NoteProducts { get; set; }
        public static ObservableCollection<Product> Products { get; set; }
        public static ObservableCollection<ProductGroup> ProductGroups { get; set; }
        public static ObservableCollection<ProductUnit> ProductUnits { get; set; }
        public static ObservableCollection<Manufacture> Manufactures { get; set; }
        public static ObservableCollection<Country> Countries { get; set; }
        public static ObservableCollection<Supplier> Suppliers { get; set; }

        public async Task<HttpResponseMessage> SendPostQueryAsync(object obj, string url)
        {
            using (HttpClient net = new HttpClient())
            using (StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))
            {
                net.DefaultRequestHeaders.Add("HWID", HWID.Instance.Get());
                net.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                return await net.PostAsync(url, content);
            }
        }
        public async Task<HttpResponseMessage> SendPutQueryAsync(object obj, string url)
        {
            using (HttpClient net = new HttpClient())
            using (StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"))
            {
                net.DefaultRequestHeaders.Add("HWID", HWID.Instance.Get());
                net.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                return await net.PutAsync(url, content);
            }
        }
        public async Task<T> GetQueryAsync<T>(string url)
        {
            using (HttpClient net = new HttpClient())
            {
                net.DefaultRequestHeaders.Add("HWID", HWID.Instance.Get());
                net.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                return JsonConvert.DeserializeObject<T>(await net.GetStringAsync(url));
            }
        }

        public async Task<HttpResponseMessage> Identity(UserLogin userLogin)
        {
            HttpResponseMessage response = await SetToken(userLogin);
            if (response.IsSuccessStatusCode)
            {
                User = JsonConvert.DeserializeObject<User>(await (await SendPostQueryAsync(userLogin, $"{AuthenticateLink}/GetUser")).Content.ReadAsStringAsync());
                RefreshToken(userLogin);
            }

            return response;
        }
        private async Task<HttpResponseMessage> SetToken(UserLogin userLogin)
        {
            HttpResponseMessage response = await SendPostQueryAsync(userLogin, $"{AuthenticateLink}/AppLogin");
            Token = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "";

            return response;
        }
        private void RefreshToken(UserLogin userLogin)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(59);
            timer.Tick += async (s, a) => { await SetToken(userLogin); };
            timer.Start();
        }
    }
}