using ProductsServer.Bot.Classes;
using ProductsServer.DataBase;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProductsServer.Bot.Commands
{
    public class StartCommand : Command
    {
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Equals(Name);
        }

        public override async Task Execute(ProductsDBContext context, Message message, TelegramBotClient botClient)
        {
            string text =
                "Привет, я бот, который поможет тебе с учётом товаров :)\n" +
                "Я умею:\n" +
                "1) Выводить список товаров (/getproducts)\n" +
                "2) Выводить поставленные товары (/getarrivalproducts)\n" +
                "3) Выводить список остатков (/getremains)";
            await botClient.SendTextMessageAsync(message.Chat.Id, text, parseMode: ParseMode.Markdown);
        }
    }
}
