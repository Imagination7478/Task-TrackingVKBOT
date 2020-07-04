#define TEST
using VkNet.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using Task_TrackingVKBOT.JSON_s;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System;

namespace Task_TrackingVKBOT.Controllers
{
    [Route("api/[controller]/[action]")] // /api/Callback/
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
            _BotLogic = new BotLogic(configuration["Config:DatabaseAddr"]);
        }

        [HttpPost]
        public IActionResult Callback([FromBody] VkJSON updates)
        {
#if TEST
            _vkApi.Messages.Send(new MessagesSendParams
            {
                UserId = 161320743,
                RandomId = new DateTime().Millisecond,
                Message = updates.Type.ToString()
            });
#endif
            // Проверяем, что находится в поле "type" 
            switch (updates.Type.Trim().ToString())
            {
                case "message_reply":
                    {
#if TEST
                        _vkApi.Messages.Send(new MessagesSendParams
                        {
                            UserId = 161320743,
                            RandomId = new DateTime().Millisecond,
                            Message = "в бд"
                        });
#endif
                        break;
                    }
                case "message_new":
                    {
                        var msg = Message.FromJson(new VkResponse(updates.Object));
#if TEST
                        _vkApi.Messages.Send(new MessagesSendParams
                        {
                            UserId = 161320743,
                            RandomId = new DateTime().Millisecond,
                            Message = "в бд"
                        });
#endif

                        switch (msg.Text.ToLower())
                        {

                            case "начать":
                                if (_BotLogic.UserInsideDatabase(msg.FromId))
                                {
                                    _BotLogic.Subscribe(_vkApi, msg.FromId, updates.GroupId, 0);
#if TEST
                                    //Если пользователь уже в базе данных, то...
                                    _vkApi.Messages.Send(new MessagesSendParams
                                    {
                                        RandomId = new DateTime().Millisecond,
                                        PeerId = msg.PeerId.Value,
                                        Message = "в бд"
                                    });
#endif
                                }
                                else
                                {
                                    _BotLogic.AddIntoDatabase();
#if TEST
                                    _vkApi.Messages.Send(new MessagesSendParams
                                    {
                                        RandomId = new DateTime().Millisecond,
                                        PeerId = msg.PeerId.Value,
                                        Message = "не в бд"
                                    });
#endif
                                    //_BotLogic.Subscribe(_vkApi, msg., updates.GroupId, 0); // 0 - предложить
                                    // Если такого пользователя нет, то предложить ему внести себя в БД, а также 
                                    // предложить ввести настоящую Имя/Фамилию при необходимости
                                }
                                break;
                            case "подписаться":
                                _BotLogic.Subscribe(_vkApi, msg.FromId, updates.GroupId, 1); // 1 - подписать
                                break;
                            case "отписаться":
                                _BotLogic.Subscribe(_vkApi, msg.FromId, updates.GroupId, 2); // 2 - отписать
                                break;
                        }

                        break;
                    }
                case "confirmation":
                    { // Если запрос на подтверждение для callback - подтверждаем
#if TEST
                        _vkApi.Messages.Send(new MessagesSendParams
                        {
                            UserId = 161320743,
                            RandomId = new DateTime().Millisecond,
                            Message = "в бд"
                        });
#endif
                        return Ok(_configuration["Config:Confirmation"]);
                    }
            }
            return Ok("ok");
        }


    }


}
