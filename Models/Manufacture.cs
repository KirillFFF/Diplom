using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsServer.Models
{
    [Table("Производители")]
    public class Manufacture
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Наименование", TypeName = "Varchar(30)")]
        public string Name { get; set; }

        [Column("Страна")]
        [ForeignKey("Страна")]
        public Country Country { get; set; }
    }
}
