using Microsoft.AspNetCore.Mvc;
using ProductsServer.Bot.Classes;
using ProductsServer.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProductsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotController : ControllerBase
    {
        private readonly ProductsDBContext _context;

        public TelegramBotController(ProductsDBContext context)
        {
            _context = context;
        }

        private ProductsDBContext Context => _context;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"Привет, я телеграм бот :)\nДля использования моих возможностей переходи в телеграм и введи в поиск: @Ychet_Tovarov_Bot");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null || update?.Message == null) 
                return Ok();

            Message message = update.Message;
            TelegramBotClient botClient = TelegramBot.BotClient;
            IReadOnlyList<Command> commands = TelegramBot.Commands;
            
            foreach (Command command in commands)
            {
                if (command.Contains(message))
                {
                    await command.Execute(Context, message, botClient);
                    break;
                }
            }

            return Ok();
        }
    }
}
