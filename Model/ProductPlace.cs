namespace ProductsClient.Model
{
    public class ProductPlace
    {
        public ProductPlace()
        {
            Product = new ArrivalProduct();
            StorageType = new StorageType();
        }

        public uint Id { get; set; }
        public ArrivalProduct Product { get; set; }
        public StorageType StorageType { get; set; }
        public uint PositionId { get; set; }
        public uint PlaceId { get; set; }
    }
}
