namespace ProductsServer.Models
{
    public class ProductWriteOff
    {
        public uint Id { get; set; }
        public ArrivalProduct Product { get; set; }
        public float Count { get; set; }
        public string Reason { get; set; }
        public WriteOffProtocol WriteOffProtocol { get; set; }
    }
}