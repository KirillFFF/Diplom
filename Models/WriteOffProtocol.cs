namespace ProductsServer.Models
{
    public class WriteOffProtocol
    {
        public uint Id { get; set; }
        public Employee Employee { get; set; }
        public string Date { get; set; }
    }
}