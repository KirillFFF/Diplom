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
    public class GetProductsCommand : Command
    {
        public override string Name => @"/getproducts";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Equals(Name);
        }

        public override async Task Execute(ProductsDBContext context, Message message, TelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Так-с... Пошёл искать, сейчас вернусь");

            List<Product> list = await context.Products.Select(x => new Product
            {
                IdName = x.IdName,
                Name = x.Name,
                Group = x.Group,
                Unit = x.Unit,
                Manufacture = new Manufacture
                {
                    Name = x.Manufacture.Name,
                    Country = x.Manufacture.Country
                }
            }).ToListAsync();
                
                //await ManageDataDB.Instance.GetProductsAsync();
            string text = $"{(list.Count > 0 ? $"{message.From.FirstName}, я нашёл для тебя товары, которые хранятся в БД:\n" : "Увы, но товаров нет")}";
            list.ForEach(product =>
            {
                text +=
                $"\nАртикул: {product.IdName}\n" +
                $"Наименование: {product.Name}\n" +
                $"Товарная группа: {product.Group.Name}\n" +
                $"Единица измерения: {product.Unit.Name}\n" +
                $"Производитель: {product.Manufacture.Name}\n" +
                $"Страна: {product.Manufacture.Country.ShortName}\n";
            });

            await botClient.SendTextMessageAsync(message.Chat.Id, text);
        }
    }
}