using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using Task_TrackingVKBOT.JSON_s;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace Task_TrackingVKBOT.Controllers
{
    [Route("api/[controller]")] // /api/Callback/
    [ApiController]
    public class CallBackController : ControllerBase
    {
        /// Конфигурация приложения
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;
        private readonly BotLogic _BotLogic;


        public CallBackController(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
            _BotLogic = new BotLogic(_configuration["Config: DatabaseAddr"]);
        }

        [HttpPost]
        public IActionResult Callback([FromBody] VkJSON updates)
        {
            // Проверяем, что находится в поле "type" 
            switch (updates.Type)
            {
                case "confirmation": // Если запрос на подтверждение для callback - подтверждаем
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    var msg = Message.FromJson(new VkResponse(updates.Object));
                    if (msg.Text != null)
                    {
                        if (_BotLogic.UserInsideDatabase(msg.FromId))
                        {
                            // Если пользователь уже в базе данных, то...
                        }
                        else
                        {
                            // Если такого пользователя нет, то предложить ему внести себя в БД, а также 
                            // предложить ввести настоящую Имя/Фамилию при необходимости

                        }
                    }

            }
            return Ok("ok");
        }
    }
}
