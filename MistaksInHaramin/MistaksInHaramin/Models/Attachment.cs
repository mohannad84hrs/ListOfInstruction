using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Attachment
    {
        [JsonProperty("id")]
        public int Id { set; get; }
        [JsonProperty("diskName")]
        public string DiskName { set; get; }
        [JsonProperty("fileName")]
        public string FileName { set; get; }
        [JsonProperty("title")]
        public string Title { set; get; }
        [JsonProperty("description")]
        public string Description { set; get; }
        [JsonProperty("createdAt")]
        public int  Created_at { set; get; }
        [JsonProperty("updatedAt")]
        public int Updated_at { set; get; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { set; get; }
    }
}
