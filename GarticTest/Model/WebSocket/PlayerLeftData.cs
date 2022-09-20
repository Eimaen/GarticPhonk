using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class PlayerLeftData
    {
        [JsonProperty("userLeft")]
        public int Id;

        [JsonProperty("newOwner")]
        public int NewOwnerId;
    }
}
