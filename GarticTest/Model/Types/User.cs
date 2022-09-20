using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Types
{
    internal class User
    {
        [JsonProperty("uid")]
        public string Uid;

        [JsonProperty("authId")]
        public string AuthId;

        [JsonProperty("mirror")]
        public string Mirror;

        [JsonProperty("id")]
        public int Id;

        [JsonProperty("nick")]
        public string Nick;

        [JsonProperty("avatar")]
        public string Avatar;

        [JsonProperty("viewer")]
        public bool Viewer;

        [JsonProperty("owner")]
        public bool Owner;

        [JsonProperty("points")]
        public int Points;

        [JsonProperty("change")]
        public int Change;
    }
}
