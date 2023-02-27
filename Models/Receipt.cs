using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Чеки")]
    public class Receipt
    {
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [ForeignKey("Сотрудник")]
        [Column("Сотрудник", TypeName = "int")]
        public Employee Employee { get; set; }

        [Column("Дата")]
        public DateTime Date { get; set; }
    }
}
