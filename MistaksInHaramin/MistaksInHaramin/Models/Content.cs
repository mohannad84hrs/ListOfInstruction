using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistaksInHaramin.Models
{
    public class Content : IEquatable<Content>
    {
        public int Id { set; get; }
    
        [JsonProperty("title")]
        public string Title { set; get; }

        public string Body { set; get; }
        [JsonProperty("description")]
        public string Description { set; get; }
        [JsonProperty("published")]
        public bool Published { set; get; }
        [JsonProperty("createdAt")]
        public int CreatedAt { set; get; }
        [JsonProperty("updatedAt")]
        public int UpdatedAt { set; get; }
        [JsonProperty("attachments")]
        public List<Attachment> Attachments { set; get; }
        [JsonProperty("slider")]
        public string Slider { set; get; }
        public override string ToString()
        {
            return Title;
        }
        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Content objAsPart = obj as Content;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
   
        public bool Equals(Content other)
        {
            if (other == null) return false;
            return (this.Title.Equals(other.Title));
        }
    }
    public class LanContent
    {
        public Content ar { set; get; }
      //  public Content en { set; get; }

    }
}
