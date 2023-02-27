using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Должности")]
    public class Position
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("Наименование", TypeName = "Varchar(30)")]
        public string Name { get; set; }
    }
}