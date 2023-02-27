namespace ProductsClient.Model
{
    public class Employee
    {
        public Employee()
        {
            Position = new Position();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
