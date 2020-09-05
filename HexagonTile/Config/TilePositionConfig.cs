using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonTile.Config
{
   
    class TilePositionConfig
    {
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
    }
}
