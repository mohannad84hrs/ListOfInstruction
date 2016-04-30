using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Relation
    {
        [JsonProperty("place")]
        public int[] Place { set; get; } = { -1 };
    }
}
