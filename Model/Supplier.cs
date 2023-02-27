namespace ProductsClient.Model
{
    public class Supplier
    {
        public Supplier()
        {

        }

        public Supplier(Supplier supplier)
        {
            Id = supplier.Id;
            Name = supplier.Name;
            Address = supplier.Address;
            Phone = supplier.Phone;
            Mail = supplier.Mail;
            Contact = supplier.Contact;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Contact { get; set; }
    }
}
