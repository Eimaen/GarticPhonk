using GarticTest.Model.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class GameData
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("screen")]
        public int Screen;

        [JsonProperty("countDown")]
        public bool CountDown;

        [JsonProperty("user")]
        public User User;

        [JsonProperty("users")]
        public List<User> Users;

        [JsonProperty("turnNum")]
        public int TurnNum;

        [JsonProperty("turnMax")]
        public int TurnMax;

        [JsonProperty("roundNum")]
        public int RoundNum;

        [JsonProperty("bookNum")]
        public int BookNum;

        [JsonProperty("bookAutomatic")]
        public bool BookAutomatic;

        [JsonProperty("bookVoice")]
        public bool BookVoice;

        [JsonProperty("bookOrder")]
        public bool BookOrder;

        [JsonProperty("configs")]
        public object Configs;

        [JsonProperty("animationConfigs")]
        public object AnimationConfigs;

        [JsonProperty("sentence")]
        public string Sentence;

        [JsonProperty("timeStarted")]
        public bool TimeStarted;
    }
}
