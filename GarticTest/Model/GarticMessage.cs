using GarticTest.Model.Messages;
using GarticTest.Model.Messages.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model
{
    public enum GarticMessageType
    {
        Unknown = 0,
        PlayerJoin = 2,
        PlayerLeft = 3,
        PingResponse = 100,
        PingProbeResponse = 101,
        GamemodeSwitch = 26,
        Start = 5,
        GameTurn = 11,
        TextInput = 6,
        Draw = 7,
        Submit = 15,
        Book = 9,
        ResetGame = 20,
        GameEnd = 24,
        Disconnect = 14
    }

    internal abstract class GarticMessage
    {
        public abstract GarticMessageType Type { get; }

        public abstract string Serialize();

        public static GarticMessage Parse(string message)
        {
            if (message == "3")
                return new GarticPingResponseMessage();
            if (message == "3probe")
                return new GarticPingProbeResponseMessage();

            if (message.StartsWith("42"))
            {
                object[] packetData = JsonConvert.DeserializeObject<object[]>(message.Substring(2));
                int something = int.Parse(packetData[0].ToString()); // 2
                GarticMessageType packetId = (GarticMessageType)int.Parse(packetData[1].ToString());
                object data;
                if (packetData.Length > 2)
                    data = packetData[2];
                else
                    data = null;

                if (packetId == GarticMessageType.GamemodeSwitch)
                    return new GarticGamemodeSwitchMessage(data);
                if (packetId == GarticMessageType.PlayerJoin)
                    return new GarticPlayerJoinMessage(data);
                if (packetId == GarticMessageType.GameTurn)
                    return new GarticGameTurnMessage(data);
                if (packetId == GarticMessageType.Start)
                    return new GarticGameStartMessage();
                if (packetId == GarticMessageType.Submit)
                    return new GarticServerSubmitMessage(data);
                if (packetId == GarticMessageType.Disconnect)
                    return new GarticDisconnectMessage(data);
            }

            return new GarticUnknownMessage();
        }
    }
}
