using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsServer.Models
{
    [Table("Пользователи")]
    public class User
    {
        [Key]
        [Column("Логин", TypeName = "Varchar(15)")]
        public string Login { get; set; }

        [ForeignKey("Сотрудник")]
        [Column("Сотрудник", TypeName = "int")]
        public Employee Employee { get; set; }

        [ForeignKey("Доступ")]
        [Column("Доступ", TypeName = "int")]
        public AccessLevel AccessLevel { get; set; }

        [JsonIgnore]
        [Column("HWID", TypeName = "Varbinary(200)")]
        public string HWID { get; set; }

        [JsonIgnore]
        [Column("Пароль", TypeName = "Varbinary(200)")]
        public string Password { get; set; }

        [JsonIgnore]
        public int? ChatId { get; set; }
    }
}