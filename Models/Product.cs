using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Товары")]
    public class Product
    {
        [Key]
        [Column("Артикул", TypeName = "Varchar(20)")]
        public string IdName { get; set; }

        [Column("Наименование", TypeName = "Varchar(30)")]
        public string Name { get; set; }

        [ForeignKey("Товарная группа")]
        [Column("Товарная группа", TypeName = "int")]
        public ProductGroup Group { get; set; }

        [ForeignKey("Единица измерения")]
        [Column("Единица измерения", TypeName = "int")]
        public ProductUnit Unit { get; set; }

        [ForeignKey("Производитель")]
        [Column("Производитель", TypeName = "int")]
        public Manufacture Manufacture { get; set; }

        [JsonIgnore]
        public ArrivalProduct ArrivalProduct { get; set; }
    }
}