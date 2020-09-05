using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace HexagonTile.Config
{
    class SysConfig
    {
        [JsonProperty("tile")]
        public TileConfig Tile { get; set; } = new TileConfig();
        [JsonProperty("position")]
        public List<TilePositionConfig> Position { get; set; } = new List<TilePositionConfig>();

        static SysConfig instance = null;
        public static SysConfig Instance
        {
            get
            {
                if (instance == null && System.IO.File.Exists("config.json"))
                {
                    instance = Newtonsoft.Json.JsonConvert.DeserializeObject<SysConfig>(System.IO.File.ReadAllText("config.json"));
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
