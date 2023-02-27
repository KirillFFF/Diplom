using Microsoft.Extensions.Configuration;
using ProductsServer.Bot.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ProductsServer.Bot.Classes
{
    public class TelegramBot
    {
        private readonly IConfiguration _configuration;
        private static TelegramBotClient _botClient;
        private static List<Command> _commandsList;

        public TelegramBot(IConfiguration configuration)
        {
            _configuration = configuration;
            _botClient = GetBotAsync().Result;
        }

        public static TelegramBotClient BotClient => _botClient;
        public static IReadOnlyList<Command> Commands => _commandsList.AsReadOnly();

        public async Task<TelegramBotClient> GetBotAsync()
        {
            if (_botClient != null)
                return _botClient;

            _commandsList = new();
            _commandsList.Add(new StartCommand());
            _commandsList.Add(new GetProductsCommand());
            _commandsList.Add(new GetArrivalProductsCommand());
            _commandsList.Add(new RemainsCommand());

            _botClient = new TelegramBotClient(_configuration["TelegramBot:Token"]);
            await _botClient.SetWebhookAsync(_configuration["TelegramBot:Host"]);
            return _botClient;
        }
    }
}
