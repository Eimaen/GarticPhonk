using GarticTest.Model.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    internal class GarticServerSubmitMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.Submit;

        public int UserId;
        public bool Ready;

        public GarticServerSubmitMessage(object data)
        {
            var messageData = JsonConvert.DeserializeObject<ReadyData>(data.ToString());
            UserId = messageData.User;
            Ready = messageData.Ready;
        }

        public GarticServerSubmitMessage() { }
    }
}
