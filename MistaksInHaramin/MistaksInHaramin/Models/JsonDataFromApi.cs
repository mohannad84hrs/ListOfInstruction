using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class JsonDataFromApi
    {
        [JsonProperty("total")]
        public int Total { set; get; }
        [JsonProperty("limit")]
        public int Limit { set; get; }
        [JsonProperty("currentPage")]
        public int CurrentPage { set; get; }
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { set; get; }
        [JsonProperty("previousPageUrl")]
        public string PreviousPageUrl { set; get; }
        [JsonProperty("lastPageUrl")]
        public string LastPageUrl { set; get; }
        [JsonProperty("data")]
        public List<Post> Data { set; get; }
    }
}
