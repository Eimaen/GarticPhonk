using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class RawDrawData
    {
        [JsonProperty("t")]
        public int Type; // Unused here, use js for better results

        [JsonProperty("d")]
        public int MessageType; // Don't know what it does

        [JsonProperty("v")]
        public object Value;
    }
}
