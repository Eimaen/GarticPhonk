using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class PlayerJoinData
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("nick")]
        public string Nickname;

        [JsonProperty("avatar")]
        public string Avatar;

        [JsonProperty("owner")]
        public bool Owner;

        [JsonProperty("viewer")]
        public bool Spectator;

        [JsonProperty("points")]
        public int Points;

        [JsonProperty("change")]
        public int Change;
    }
}
