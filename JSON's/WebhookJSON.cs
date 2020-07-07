using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Task_TrackingVKBOT.JSON_s
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    [Serializable]
    public class WebhookJSON
    {
        public string issue_event_type_name { get; set; }
        public Changelog changelog { get; set; }
        public Issue issue { get; set; }

    }

    [Serializable]
    public class Item
    {
        public string field { get; set; }
        public string fieldtype { get; set; }
        public string fieldId { get; set; }
        public string from { get; set; }
        public string fromString { get; set; }
        public string to { get; set; }
        public string toString { get; set; }

    }

    [Serializable]
    public class Changelog
    {
        public string id { get; set; }
        public List<Item> items { get; set; }

    }

    [Serializable]
    public class Fields
    {
        public Project project { get; set; }
    }

    [Serializable]
    public class Issue
    {
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }

    }

    [Serializable]
    public class Project
    {
        public string self { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string projectTypeKey { get; set; }
        public bool simplified { get; set; }

    }

    [Serializable]
    public class User
    {
        public string self { get; set; }
        public string accountId { get; set; }

        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }

    }

    [Serializable]
    public class Issuetype
    {
        public string self { get; set; }
        public string id { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public bool subtask { get; set; }
        public int avatarId { get; set; }
        public string entityId { get; set; }

    }

    

    [Serializable]
    public class Issuerestrictions
    {

    }

    [Serializable]
    public class Issuerestriction
    {
        public Issuerestrictions issuerestrictions { get; set; }
        public bool shouldDisplay { get; set; }

    }

    [Serializable]
    public class Watches
    {
        public string self { get; set; }
        public int watchCount { get; set; }
        public bool isWatching { get; set; }

    }

    [Serializable]
    public class Priority
    {
        public string self { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }

    }

    [Serializable]
    public class NonEditableReason
    {
        public string reason { get; set; }
        public string message { get; set; }

    }

    [Serializable]
    public class Customfield10018
    {
        public bool hasEpicLinkFieldDependency { get; set; }
        public bool showField { get; set; }
        public NonEditableReason nonEditableReason { get; set; }

    }

    [Serializable]
    public class Assignee
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }

    }

    [Serializable]
    public class StatusCategory
    {
        public string self { get; set; }
        public int id { get; set; }
        public string key { get; set; }
        public string colorName { get; set; }
        public string name { get; set; }

    }

    [Serializable]
    public class Status
    {
        public string self { get; set; }
        public string description { get; set; }
        public string iconUrl { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public StatusCategory statusCategory { get; set; }

    }

    [Serializable]
    public class Creator
    {
        public string self { get; set; }
        public string accountId { get; set; }

        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }

    }

    [Serializable]
    public class Reporter
    {
        public string self { get; set; }
        public string accountId { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        public string timeZone { get; set; }
        public string accountType { get; set; }

    }

    [Serializable]
    public class Aggregateprogress
    {
        public int progress { get; set; }
        public int total { get; set; }

    }

    [Serializable]
    public class Progress
    {
        public int progress { get; set; }
        public int total { get; set; }

    }

    [Serializable]
    public class Votes
    {
        public string self { get; set; }
        public int votes { get; set; }
        public bool hasVoted { get; set; }

    }

    
}
