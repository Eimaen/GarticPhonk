using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class ReadyData
    {
        [JsonProperty("user")]
        public int User;

        [JsonProperty("ready")]
        public bool Ready;
    }
}
