#define TEST
#define WITHOUTPARAMS
using VkNet.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using Task_TrackingVKBOT.JSON_s;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System;
using Newtonsoft.Json;

namespace Task_TrackingVKBOT.Controllers
{
    //[Route("api/[action]")]
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
            _BotLogic = new BotLogic(_vkApi, configuration["Database:DataSource"], configuration["Database:UserID"], configuration["Database:Password"], configuration["Database:InitialCatalog"]);
        }

        [HttpPost("/webhook")]
        public IActionResult Webhook([FromBody] WebhookJSON updates) //[FromBody] WebhookJSON updates
        {
            if (updates.issue_event_type_name == "issue_assigned")
            {
                //Если тип изменения issue_assigned(изменен исполнитель)
                if (updates.changelog.items[0].fromString != updates.changelog.items[0].toString)
                {
                    _BotLogic.Change(updates.changelog.items[0].fromString, updates.changelog.items[0].toString, updates);
                    return Ok();
                }
                else
                    return Ok();
                    
            }
            else
                return Ok();

        }

        [HttpPost("/callback")]
        public IActionResult Callback([FromBody] VkJSON updates) // 
        {
            switch (updates.type)
            {
                case "message_new":
                    {
                        var msg = updates.Object;

                        switch (msg.text.ToLower().Trim())
                        {
                            case "начать":
                                if (_BotLogic.UserInsideDatabase(msg.from_id))
                                {
                                    _BotLogic.Subscribe(msg.from_id, 0);

                                    //Если пользователь уже в базе данных, то...
                                    _vkApi.Messages.Send(new MessagesSendParams
                                    {
                                        RandomId = new DateTime().Millisecond,
                                        PeerId = msg.from_id,
                                        Message = "Ваши данные уже внесены в базу."
                                    });
                                    break;
                                }
                                else
                                {
                                    _BotLogic.AddIntoDatabase(msg.from_id, updates.group_id);

                                    _BotLogic.Subscribe(updates.group_id, 0); // 0 - предложить
                                    // Если такого пользователя нет, то предложить ему внести себя в БД, а также 
                                    // предложить ввести настоящую Имя/Фамилию при необходимости
                                    break;
                                }
                            case "подписаться":
                                _BotLogic.Subscribe(msg.from_id, 1); // 1 - подписать
                                break;
                            case "отписаться":
                                _BotLogic.Subscribe(msg.from_id, 2); // 2 - отписать
                                break;
                            default:
                                _vkApi.Messages.Send(new MessagesSendParams
                                {
                                    RandomId = new DateTime().Millisecond,
                                    PeerId = msg.from_id,
                                    Message = "Мой создатель меня к такому не готовил..."
                                });
                                return Ok("ok");
                        }
                        return Ok("ok");
                    }

                case "confirmation":
                    { // Если запрос на подтверждение для callback - подтверждаем
                        return Ok(_configuration["Config:Confirmation"]);
                    }
                default:
                    return Ok("ok");
            }
        }
    }
}