using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Расход товаров")]
    public class SaleProduct
    {
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [ForeignKey("Товар")]
        [Column("Товар", TypeName = "int")]
        public ArrivalProduct Product { get; set; }

        [ForeignKey("Чек")]
        [Column("Чек", TypeName = "int")]
        public Receipt Receipt { get; set; }

        [Column("Количество")]
        public float Count { get; set; }
    }
}