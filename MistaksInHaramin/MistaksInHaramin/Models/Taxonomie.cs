using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Taxonomie
    {
        [JsonProperty("category")]
        public int [] Category { set; get; }
        [JsonProperty("area")]
        public int[] Area { set; get; } = { -1 };
    }
}
