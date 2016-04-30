using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Media
    {
        [JsonProperty("id")]
        public int Id { set; get; }
        [JsonProperty("title")]
        public string title { set; get; }
        [JsonProperty("type")]
        public string type { set; get; }
        [JsonProperty("embeddedId")]
        public string embeddedId { set; get; }
        [JsonProperty("embeddedLink")]
        public string embeddedLink { set; get; }

    }
}
