using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Task_TrackingVKBOT
{
    [Serializable]
    public class JSON
    {
        /// Тип события
        [JsonProperty("issue_event_type_name")]
        public string TypeWebHook { get; set; }

        [JsonProperty("type")]
        public string TypeCallback { get; set; }

        // Записи в changelog'е
        [JsonProperty("changelog")]
        public Changelog changelog { get; set; }
    }
    public class Item
    {
        // Кто был прошлым исполнителем
        public string fromString { get; set; }
        // Кто стал новым
        public string toString { get; set; }

    }

    public class Changelog
    {
        public string id { get; set; }
        public Item items { get; set; }

    } 
}
