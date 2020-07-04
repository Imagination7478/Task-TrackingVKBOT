using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Task_TrackingVKBOT.JSON_s
{
	[Serializable]
	public class VkJSON
	{
		/// <summary>
		/// Тип события
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }

		/// <summary>
		/// Объект, инициировавший событие
		/// Структура объекта зависит от типа уведомления
		/// </summary>

		[JsonProperty("object")]
		public VkObject Object { get; set; }

		/// <summary>
		/// ID сообщества, в котором произошло событие
		/// </summary>
		[JsonProperty("group_id")]
		public long GroupId { get; set; }

		/// <summary>
		/// Секретный ключ. Передается с каждым уведомлением от сервера
		/// </summary>
		[JsonProperty("secret")]
		public string Secret { get; set; }
	}

    public class Message
    {
        [JsonProperty("fromid")]
        public long? FromId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
    public class VkObject
    {
        [JsonProperty("message")]
        public Message Messages { get; set; }
    }

}
