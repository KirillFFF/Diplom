namespace ProductsClient.Model
{
    public class ProductUnit
    {
        public ProductUnit()
        {

        }

        public ProductUnit(ProductUnit unit)
        {
            Id = unit.Id;
            Name = unit.Name;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
