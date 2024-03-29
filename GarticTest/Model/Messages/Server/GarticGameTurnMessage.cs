﻿using GarticTest.Model.Enums;
using GarticTest.Model.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    internal class GarticGameTurnMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.GameTurn;

        public TurnType TurnType;
        public int Number, PreviousTurnId, PreviousUserId;
        public string Sentence, PreviousUserNickname;
        public object PreviousTurnData; // ?

        public GarticGameTurnMessage(object dataRaw)
        {
            string data = dataRaw.ToString();
            Console.WriteLine($"dd: {data}");
            var turnData = JsonConvert.DeserializeObject<TurnData>(data);
            TurnType = turnData.TurnType;
            Number = turnData.TurnNumber;
            Sentence = turnData.Sentence;
            if (turnData.Previous != null)
            {
                PreviousTurnId = turnData.Previous.Id;
                PreviousUserId = turnData.Previous.User.Id;
                PreviousUserNickname = turnData.Previous.User.Nickname;
                PreviousTurnData = turnData.Previous.Data;
            }
        }
    }
}
