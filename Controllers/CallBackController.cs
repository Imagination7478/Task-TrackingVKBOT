using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using Task_TrackingVKBOT.JSON_s;

namespace Task_TrackingVKBOT.Controllers
{
    [Route("api/[controller]")] // /api/Callback/
    [ApiController]
    public class CallBackController : ControllerBase
    {
        /// Конфигурация приложения
        private readonly IConfiguration _configuration;

        public CallBackController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] VkJSON updates)
        {
            // Проверяем, что находится в поле "type" 
            if (updates.Type == "confirmation")
            {
                // Если запрос на подтверждение для callback - подтверждаем
                return Ok(_configuration["Config:Confirmation"]);
            }
            return Ok("ok");
        }
    }
}
