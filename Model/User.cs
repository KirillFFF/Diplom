namespace ProductsClient.Model
{
    public class User
    {
        public User()
        {
            Employee = new Employee();
            AccessLevel = new AccessLevel();
        }

        public Employee Employee { get; set; }
        public string Login { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
