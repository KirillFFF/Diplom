using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("ТТН")]
    public class NoteProduct
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [ForeignKey("Поставщик")]
        [Column("Поставщик", TypeName = "int")]
        public Supplier Supplier { get; set; }

        [ForeignKey("Сотрудник")]
        [Column("Сотрудник", TypeName = "int")]
        public Employee Employee { get; set; }

        [Column("Дата прихода")]
        public DateTime Date { get; set; }
    }
}