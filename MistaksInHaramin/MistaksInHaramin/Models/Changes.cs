using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Changes
    {
        [JsonProperty("created")]
        public List<Post> Created;
        [JsonProperty("updated")]
        public List<Post> Updated;
        [JsonProperty("deleted")]
        public List<Post> Deleted;
    }
}
