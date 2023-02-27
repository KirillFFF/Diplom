namespace ProductsClient.Model
{
    public class Country
    {
        public Country()
        {

        }

        public Country(Country country)
        {
            Id = country.Id;
            IdName = country.IdName;
            ShortName = country.ShortName;
            FullName = country.FullName;
        }

        public uint Id { get; set; }
        public string IdName { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
