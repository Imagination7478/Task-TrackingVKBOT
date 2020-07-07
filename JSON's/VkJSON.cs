using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Task_TrackingVKBOT.JSON_s
{
    [Serializable]
    public class VkJSON
    {
        public string type { get; set; }
        public Object Object { get; set; }
        public long group_id { get; set; }
        public string Secret { get; set; }
    }

    [Serializable]
    public class Object
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int Out { get; set; }
        public int peer_id { get; set; }
        public string text { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public int random_id { get; set; }
        public List<object> attachments { get; set; }
        public bool is_hidden { get; set; }

    }

   [Serializable]
    public class Message
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int Out { get; set; }
        public int peer_id { get; set; }
        public string text { get; set; }
        public int conversation_message_id { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public int random_id { get; set; }
        public List<object> attachments { get; set; }
        public bool is_hidden { get; set; }

    }
}
