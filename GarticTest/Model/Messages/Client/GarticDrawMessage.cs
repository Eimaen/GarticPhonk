using GarticTest.Model.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Client
{
    public enum DrawMessageType
    {
        Begin = 1,
        Tick = 3,
        End = 3
    }

    internal class GarticDrawMessage : GarticClientMessage
    {
        public override GarticMessageType Type => GarticMessageType.Draw;

        public DrawMessageType DrawMessageType;
        public object DrawData;
        public int TurnNumber;

        public GarticDrawMessage(DrawMessageType messageType, int turn, object drawData)
        {
            DrawMessageType = messageType;
            DrawData = drawData;
            TurnNumber = turn;
        }

        public override string Serialize()
        {
            return $"42[2,{(int)Type},{JsonConvert.SerializeObject(new RawDrawData() { Type = TurnNumber, MessageType = (int)DrawMessageType, Value = DrawData })}]";
        }
    }
}
