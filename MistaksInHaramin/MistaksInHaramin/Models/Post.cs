using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Post :IEquatable<Post> 
    {
        [JsonProperty("id")]
        public int Id { set; get; }
        [JsonProperty("type")]
        public string Type { set; get; }
        [JsonProperty("published")]
        public bool Published { set; get; }
        [JsonProperty("longitude")]
        public string Longitude { set; get; } 
        [JsonProperty("latitude")]
        public string Latitude { set; get; }
        [JsonProperty("createdAt")]
        public int CreatedAt { set; get; }
        [JsonProperty("updatedAt")]
        public int UpdatedAt { set; get; }
        [JsonProperty("content")]
        public LanContent Content { set; get; }
        [JsonProperty("media")]
        public List<Media> media { set; get; }
        [JsonProperty("taxonomies")]
        public Taxonomie Taxonomies { set; get; } = null;
        [JsonProperty("relations")]
        public Relation Relations { set; get; } = null;
        [JsonProperty("in_home")]
        public bool In_home { set; get; }
        public override string ToString()
        {
            return "ID: " + Id + "   Name: " + Type;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Post objAsPart = obj as Post;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return Id;
        }
        public bool Equals(Post other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }

    }
}
