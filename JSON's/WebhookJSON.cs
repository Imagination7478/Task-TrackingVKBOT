using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Task_TrackingVKBOT.JSON_s
{
	[Serializable]
    public class WebhookJSON
    {
		// Инициатор события
		[JsonProperty("object")]
		public JObject Object1 { get; set; }

		// Тип события
		[JsonProperty("issue_event_type_name")]
		public string Type { get; set; }

		// Записи в changelog'е
		[JsonProperty("changelog")]
		public Changelog changelog { get; set; }
	}
	public class Item
	{
		[JsonProperty("fromString")]
		// Кто был прошлым исполнителем
		public string fromString { get; set; }
		[JsonProperty("toString")]
		// Кто стал новым
		public string toString { get; set; }
	}

	public class Changelog
	{
		public string id { get; set; }
		public Item items { get; set; }
	}
}
