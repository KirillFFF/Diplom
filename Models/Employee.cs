using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Сотрудники")]
    public class Employee
    {
        [Key]
        [Column("Код", TypeName = "int")]
        public uint Id { get; set; }

        [Column("ФИО", TypeName = "Varchar(60)")]
        public string Name { get; set; }

        [Column("Телефон", TypeName = "Varchar(12)")]
        public string Phone { get; set; }

        [Column("Адрес", TypeName = "Varchar(60)")]
        public string Address { get; set; }

        [ForeignKey("Должность")]
        [Column("Должность", TypeName = "int")]
        public Position Position { get; set; }
    }
}