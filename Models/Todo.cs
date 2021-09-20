using System;
using Newtonsoft.Json;

namespace assignment.Models
{
    public class Todo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("dueBy")]
        public DateTime DueBy {get; set;}
    }
}