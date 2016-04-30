using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class MainCategories
    {
        [JsonProperty("data")]
        public List<Cateogry> CategoryList { set; get; }
    }
   public class Cateogry
    {
        [JsonProperty("id")]
        public int Id { set; get; }
        [JsonProperty("type")]
        public string Type { set; get; }
        [JsonProperty("posts")]
        public int [] Posts { set; get; }
        [JsonProperty("createdAt")]
        public int CreatedAt { set; get; }
        [JsonProperty("updatedAt")]
        public int UpdatedAt { set; get; }
        [JsonProperty("content")]
        public LanContent Contents { set; get; }
    }
}
