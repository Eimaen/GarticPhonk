using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.WebSocket
{
    internal class TextInputData
    {
        [JsonProperty("t")]
        public int Timestamp; // ?

        [JsonProperty("v")]
        public string Text;
    }
}
