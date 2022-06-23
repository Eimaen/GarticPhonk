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
    internal class GarticPlayerJoinMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.PlayerJoin;

        public string Nickname, Avatar;
        public int Id, Points, Change;
        public bool Owner, Spectator;

        public GarticPlayerJoinMessage(object data)
        {
            var messageData = JsonConvert.DeserializeObject<PlayerJoinData>(data.ToString());
            Nickname = messageData.Nickname;
            Avatar = messageData.Avatar;
            Id = messageData.Id;
            Points = messageData.Points;
            Change = messageData.Change;
            Owner = messageData.Owner;
            Spectator = messageData.Spectator;
        }
    }
}
