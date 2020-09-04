using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace HexagonTile.config
{
    class TileConfig
    {
        [JsonProperty("width")]
        public int Width { get; set; } = 64;
        [JsonProperty("height")]
        public int Height { get; set; } = 64;
    }
}
