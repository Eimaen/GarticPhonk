using GarticTest.Model.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    internal class GarticPlayerLeftMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.PlayerJoin;

        public int Id, NewOwnerId;

        public GarticPlayerLeftMessage(object data)
        {
            var messageData = JsonConvert.DeserializeObject<PlayerLeftData>(data.ToString());
            Id = messageData.Id;
            NewOwnerId = messageData.NewOwnerId;
        }
    }
}
