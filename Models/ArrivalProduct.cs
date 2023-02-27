using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Приход товаров")]
    public class ArrivalProduct
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [ForeignKey("ТТН")]
        [Column("ТТН", TypeName = "int")]
        public NoteProduct Note { get; set; }

        [ForeignKey("IdName")]
        public Product Product { get; set; }

        [Column("Количество")]
        public float Count { get; set; }

        [Column("Годен до")]
        public DateTime ValidUntil { get; set; }

        [Column("Цена закупки")]
        public float PurchaseCost { get; set; }

        [Column("Цена продажи")]
        public float SellCost { get; set; }

        [JsonIgnore]
        [ForeignKey("Товар")]
        [Column("Товар", TypeName = "Varchar(20)")]
        public string IdName { get; set; }
    }
}