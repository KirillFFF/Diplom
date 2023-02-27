using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Поставщики")]
    public class Supplier
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Наименование", TypeName = "Varchar(30)")]
        public string Name { get; set; }

        [Column("Юридический адрес", TypeName = "Varchar(60)")]
        public string Address { get; set; }

        [Column("Телефон", TypeName = "Varchar(12)")]
        public string Phone { get; set; }

        [Column("Почта", TypeName = "Varchar(30)")]
        public string Mail { get; set; }

        [Column("Контактное лицо", TypeName = "Varchar(30)")]
        public string Contact { get; set; }
    }
}