using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Task_TrackingVKBOT
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;

        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] JSON updates)
        {
            // Проверяем, что находится в поле "type" 
            if (updates.TypeCallback == "confirmation")
            {
                // Если запрос на подтверждение для callback - подтверждаем
                return Ok(_configuration["Config:Confirmation"]);
            }
            else if (updates.TypeWebHook == "issue_assigned") {
                // Если это json вебхука, то обрабатываем его
                if (updates.changelog.items.fromString != updates.changelog.items.toString)
                {
                    BotLogic Bot = new BotLogic();
                    Bot.Change(updates.changelog.items.fromString, updates.changelog.items.toString);
                    // TODO: Обработать ошибку
                }
                // Отправляем строку для подтверждения 
                return Ok(_configuration["Config:Confirmation"]);
            }
            return Ok("ok");
        }
    }
}
