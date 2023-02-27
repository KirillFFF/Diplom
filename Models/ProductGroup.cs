using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Товарные группы")]
    public class ProductGroup
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Наименование", TypeName = "Varchar(25)")]
        public string Name { get; set; }
    }
}