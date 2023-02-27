using ProductsServer.Bot.Classes;
using ProductsServer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProductsServer.Bot.Commands
{
    public class LoginCommand : Command
    {
        public override string Name => "/login";

        public override bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Equals(Name);
        }

        public override Task Execute(ProductsDBContext context, Message message, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }
    }
}
