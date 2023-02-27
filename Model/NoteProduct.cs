using System;

namespace ProductsClient.Model
{
    public class NoteProduct
    {
        public NoteProduct()
        {
            Employee = new Employee();
            Supplier = new Supplier();
        }

        public NoteProduct(NoteProduct note)
        {
            Id = note.Id;
            Employee = note.Employee;
            Date = note.Date;
            Supplier = note.Supplier;
        }

        public uint Id { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public Supplier Supplier { get; set; }
    }
}
