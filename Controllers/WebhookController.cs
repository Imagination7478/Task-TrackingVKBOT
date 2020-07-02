using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using Task_TrackingVKBOT.JSON_s;

namespace Task_TrackingVKBOT.Controllers
{
    [Route("api/[controller]")] // /api/Webhook/
    [ApiController]
    public class WebhookController : ControllerBase
    {
        /// Конфигурация приложения
        private readonly IConfiguration _configuration;

        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Webhook([FromBody] WebhookJSON updates)
        {
            updates = new WebhookJSON();
            if (updates.Type == "issue_assigned")
            {
                // Если тип изменения issue_assigned(изменен исполнитель)
                if (updates.changelog.items.fromString != updates.changelog.items.toString)
                {
                    BotLogic Bot = new BotLogic();
                    Bot.Change(updates.changelog.items.fromString, updates.changelog.items.toString);
                    // TODO: Обработать ошибку
                    return Ok(_configuration["200"]);
                }
            }
            return Ok("ok");
        }
    }
}
