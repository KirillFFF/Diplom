using ProductsServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProductsServer.Bot.Classes
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task Execute(ProductsDBContext context, Message message, TelegramBotClient client);
        public abstract bool Contains(Message message);
    }
}
