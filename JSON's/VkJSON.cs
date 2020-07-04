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
		public JObject Object { get; set; }

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
	//public class Message
	//{
	//	[JsonProperty("fromid")]
	//	public long? fromid { get; set; }

	//	[JsonProperty("text")]
	//	public string text { get; set; }

	//	[JsonProperty("text")]
	//	public string groupId { get; set; }
	//}
	//public class VkObject
	//{
	//	[JsonProperty("message")]
	//	public Message messages { get; set; }
	//}
	
}
