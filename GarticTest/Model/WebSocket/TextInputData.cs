using GarticTest.Model.Enums;
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
        public TextInputType Type; // 2 for guess, 0 for sentence

        [JsonProperty("v")]
        public string Text;
    }
}
