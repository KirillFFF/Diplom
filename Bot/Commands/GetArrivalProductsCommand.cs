using Microsoft.EntityFrameworkCore;
using ProductsServer.Bot.Classes;
using ProductsServer.DataBase;
using ProductsServer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProductsServer.Bot.Commands
{
    public class GetArrivalProductsCommand : Command
    {
        public override string Name => @"/getarrivalproducts";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Equals(Name);
        }

        public override async Task Execute(ProductsDBContext context, Message message, TelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Так-с... Пошёл искать, сейчас вернусь");

            List<ArrivalProduct> list = await context.ArrivalProducts.Select(x => new ArrivalProduct
            {
                Id = x.Id,
                Note = new NoteProduct
                {
                    Id = x.Note.Id
                },
                Product = new Product
                {
                    IdName = x.Product.IdName,
                    Name = x.Product.Name,
                    Group = x.Product.Group,
                    Unit = x.Product.Unit
                },
                Count = x.Count,
                ValidUntil = x.ValidUntil,
                PurchaseCost = x.PurchaseCost,
                SellCost = x.SellCost
            }).ToListAsync();
                //await ManageDataDB.Instance.GetArrivalProductsAsync();


            string text = $"{(list.Count > 0 ? $"{message.From.FirstName}, я нашёл для тебя товары, которые были привезены:\n" : "Увы, но товары ещё не привозили :(")}";
            list.OrderBy(x => x.Id).ToList().ForEach( (arrival) =>
            {
                text +=
                $"\nНомер поставки: {arrival.Id}\n" +
                $"№ ТТН: {arrival.Note.Id}\n" +
                $"Товар: {arrival.Product.IdName} ({arrival.Product.Name})\n" +
                $"Количество: {arrival.Count} {arrival.Product.Unit.Name}\n" +
                $"Годен до: {arrival.ValidUntil:dd.MM.yyyy} ({arrival.ValidUntil.ToString("D", new CultureInfo("ru-RU"))})\n" +
                $"До истечения срока годности: {(arrival.ValidUntil - DateTime.Now).Days} дн.\n" +
                $"Цена закупки: {arrival.PurchaseCost.ToString("C2", new CultureInfo("ru-RU"))}\n" +
                $"Цена продажи: {arrival.SellCost.ToString("C2", new CultureInfo("ru-RU"))}\n" +
                $"Наценка: {Convert.ToInt32((arrival.SellCost - arrival.PurchaseCost) / arrival.PurchaseCost * 100)}%\n";
            });

            await botClient.SendTextMessageAsync(message.Chat.Id, text);
        }
    }
}
