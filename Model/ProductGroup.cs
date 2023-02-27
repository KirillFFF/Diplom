namespace ProductsClient.Model
{
    public class ProductGroup
    {
        public ProductGroup()
        {

        }

        public ProductGroup(ProductGroup group)
        {
            Id = group.Id;
            Name = group.Name;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
