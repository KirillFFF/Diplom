using System;

namespace ProductsClient.Model
{
    public class Receipt
    {
        public Receipt()
        {
            Employee = new Employee();
        }

        public uint Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
    }
}
