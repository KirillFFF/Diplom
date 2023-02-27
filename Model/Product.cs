namespace ProductsClient.Model
{
    public class Product
    {
        public Product()
        {
            Group = new ProductGroup();
            Unit = new ProductUnit();
            Manufacture = new Manufacture();
        }

        public Product(Product product)
        {
            IdName = product.IdName;
            Name = product.Name;
            Group = product.Group;
            Unit = product.Unit;
            Manufacture = product.Manufacture;
        }

        public string IdName { get; set; }
        public string Name { get; set; }
        public ProductGroup Group { get; set; }
        public ProductUnit Unit { get; set; }
        public Manufacture Manufacture { get; set; }
    }
}
