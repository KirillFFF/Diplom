namespace ProductsServer.Models
{
    public class ProductPlace
    {
        public uint Id { get; set; }
        public ArrivalProduct Product { get; set; }
        public StorageType StorageType { get; set; }
        public uint PositionId { get; set; }
        public uint PlaceId { get; set; }
    }
}