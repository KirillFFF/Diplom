namespace ProductsClient.Model
{
    public class Manufacture
    {
        public Manufacture()
        {
            Country = new Country();
        }

        public Manufacture(Manufacture manufacture)
        {
            Id = manufacture.Id;
            Name = manufacture.Name;
            Country = manufacture.Country;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
