using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Уровни доступа")]
    public class AccessLevel
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Наименование", TypeName = "Varchar(5)")]
        public string Name { get; set; }
    }
}