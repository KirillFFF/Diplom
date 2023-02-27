namespace ProductsClient.Model
{
    public class WriteOffProtocol
    {
        public WriteOffProtocol()
        {
            Employee = new Employee();
        }

        public uint Id { get; set; }
        public Employee Employee { get; set; }
        public string Date { get; set; }
    }
}
