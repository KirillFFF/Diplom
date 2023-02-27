using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsServer.Models
{
    [Table("Страны")]
    public class Country
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Буквенный код", TypeName = "Varchar(3)")]
        public string IdName { get; set; }

        [Column("Краткое наименование", TypeName = "Varchar(25)")]
        public string ShortName { get; set; }

        [Column("Полное наименование", TypeName = "Varchar(55)")]
        public string FullName { get; set; }
    }
}