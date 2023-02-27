using ProductsClient.Classes;

namespace ProductsClient.Model
{
    public class SaleProduct : BaseVM
    {
        private float _count;

        public SaleProduct()
        {
            Product = new ArrivalProduct();
            Receipt = new Receipt();
        }

        public uint Id { get; set; }
        public ArrivalProduct Product { get; set; }
        public float Count
        {
            get => _count;
            set { _count = value; OnPropertyChanged(); }
        }
        public Receipt Receipt { get; set; }
    }
}