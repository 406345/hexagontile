using HexagonTile.config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace HexagonTile
{
    class Config
    {
        [JsonProperty("tile")]
        public TileConfig Tile { get; set; } = new TileConfig();
        
        
        
        static Config instance = new Config();
        public static Config Instance
        {
            get
            {
                if (instance == null && System.IO.File.Exists("config.json"))
                {
                    instance = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText("config.json"));
                }

                return instance;
            }
        }
        public static void Save()
        {
            string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(instance);
            System.IO.File.WriteAllText("config.json", jsonStr);
        }
    }
}
