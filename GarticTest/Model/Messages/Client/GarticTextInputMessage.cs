using GarticTest.Model.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Client
{
    internal class GarticTextInputMessage : GarticMessage
    {
        public override GarticMessageType Type => GarticMessageType.TextInput;

        public string Text;

        public GarticTextInputMessage(string message)
        {
            Text = message;
        }

        public override string Serialize()
        {
            return $"42[2,{(int)Type},{JsonConvert.SerializeObject(new TextInputData { Text = Text, Timestamp = 0 })}]";
        }
    }
}
