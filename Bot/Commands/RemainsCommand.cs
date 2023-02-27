using Microsoft.EntityFrameworkCore;
using ProductsServer.Bot.Classes;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProductsServer.Bot.Commands
{
    public class RemainsCommand : Command
    {
        public override string Name => "/getremains";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Equals(Name);
        }

        public override async Task Execute(ProductsDBContext context, Message message, TelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Так-с... Пошёл искать, сейчас вернусь");

            List<ArrivalProduct> arrivals = await context.ArrivalProducts.FromSqlRaw(
                "SELECT `Приход товаров`.`Код`, `Приход товаров`.`ТТН`, `Приход товаров`.`Товар`, (`Приход товаров`.`Количество` - IFNULL(`Расход товаров`.`Количество`, 0) - IFNULL(`Списание товаров`.`Количество`, 0)) AS 'Количество', `Приход товаров`.`Годен до`, `Приход товаров`.`Цена закупки`, `Приход товаров`.`Цена продажи`" +
                "FROM `Приход товаров` LEFT JOIN `Расход товаров`" +
                "ON `Расход товаров`.`Товар` = `Приход товаров`.`Код` LEFT JOIN `Списание товаров`" +
                "ON `Списание товаров`.`Товар` = `Приход товаров`.`Код`").Select(x => new ArrivalProduct
                {
                    Id = x.Id,
                    Note = new NoteProduct
                    {
                        Id = x.Note.Id,
                        Supplier = x.Note.Supplier,
                        Employee = new Employee
                        {
                            Id = x.Note.Employee.Id,
                            Name = x.Note.Employee.Name,
                            Phone = x.Note.Employee.Phone,
                            Address = x.Note.Employee.Address,
                            Position = x.Note.Employee.Position
                        },
                        Date = x.Note.Date
                    },
                    Product = new Product
                    {
                        IdName = x.Product.IdName,
                        Name = x.Product.Name,
                        Group = x.Product.Group,
                        Unit = x.Product.Unit,
                        Manufacture = new Manufacture
                        {
                            Id = x.Product.Manufacture.Id,
                            Name = x.Product.Manufacture.Name,
                            Country = x.Product.Manufacture.Country
                        }
                    },
                    Count = x.Count,
                    ValidUntil = x.ValidUntil,
                    PurchaseCost = x.PurchaseCost,
                    SellCost = x.SellCost
                }).OrderBy(x => x.Id).ToListAsync();

            string text = $"{(arrivals.Count > 0 ? $"{message.From.FirstName}, я нашёл для тебя информацию:\n\n" : "Увы, но товары ещё не привозили :(")}";
            if (arrivals.Count > 0)
            {
                text += "Список остатков по поставкам:";
                arrivals.ForEach(x => text += $"\n{x.Product.IdName}: {x.Count} {x.Product.Unit.Name} (поставка №{x.Id})");
                text += "\n\nСписок остатков по группам:";
                arrivals.GroupBy(x => x.Product.Group).Select(x => new ArrivalProduct
                {
                    Product = new Product { Group = x.Key },
                    Count = x.Sum(y => y.Count)
                }).ToList().ForEach(x => text += $"\n{x.Product.Group.Name}: {x.Count} ед");
            }
            

            await botClient.SendTextMessageAsync(message.Chat.Id, text, parseMode: ParseMode.Markdown);
        }
    }
}
