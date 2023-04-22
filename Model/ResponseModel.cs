using Newtonsoft.Json;
using System.Collections.Generic;

namespace Primo.TiP.Model {
    // Document myDeserializedClass = JsonConvert.DeserializeObject<List<Document>>(myJsonResponse);
    public class Box
    {
        [JsonProperty("x_min")]
        public int XMin;

        [JsonProperty("y_min")]
        public int YMin;

        [JsonProperty("x_max")]
        public int XMax;

        [JsonProperty("y_max")]
        public int YMax;
    }

    public class Entity
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("display_name")]
        public string DisplayName;

        [JsonProperty("value")]
        public object Value;

        [JsonProperty("confidence")]
        public double Confidence;

        [JsonProperty("sure")]
        public bool Sure;

        [JsonProperty("locations")]
        public List<Location> Locations;
    }

    public class Group
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("entities")]
        public List<string> Entities;
    }

    public class Location
    {
        [JsonProperty("file_id")]
        public object FileId;

        [JsonProperty("page_num")]
        public int PageNum;

        [JsonProperty("value")]
        public object Value;

        [JsonProperty("box")]
        public Box Box;
    }

    public class Document
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("type")]
        public Type Type;

        [JsonProperty("entities")]
        public List<Entity> Entities;

        [JsonProperty("groups")]
        public List<Group> Groups;

        [JsonProperty("sure")]
        public bool Sure;
    }

    public class Type
    {
        [JsonProperty("value")]
        public string Value;

        [JsonProperty("confidence")]
        public double Confidence;

        [JsonProperty("sure")]
        public bool Sure;
    }
}