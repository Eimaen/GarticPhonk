using GarticTest.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class PreviousTurnData
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("user")]
        public PlayerJoinData User;

        [JsonProperty("data")]
        public object Data;
    }

    internal class TurnData
    {
        [JsonProperty("turnNum")]
        public int TurnNumber;

        [JsonProperty("screen")]
        public TurnType TurnType;

        [JsonProperty("previous")]
        public PreviousTurnData Previous;

        [JsonProperty("sentence")]
        public string Sentence;
    }
}
